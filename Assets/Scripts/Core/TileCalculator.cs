using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TileCalculation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private HexGridGenerator gridGenerator;
    [SerializeField] private GameObject worldCanvasPrefab;
    [SerializeField] private Button checkButton;
    [SerializeField] private Button cancelButton;
    
    [Header("Tile Placement")]
    [SerializeField] private Tile currentTileToPlace;
    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private Material previewMaterial;
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;

    private GameObject worldCanvasInstance;
    private Tile hoveredTile;
    private Tile selectedTile;
    private Vector3 placementPosition;
    private bool isPlacementMode;
    private bool isValidPlacement;
    private Material originalMaterial;
    
    // Store grid tiles for validation
    private Dictionary<Vector3, Tile> gridTiles = new Dictionary<Vector3, Tile>();

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        isPlacementMode = false;
        isValidPlacement = false;
    }

    void Start()
    {
        SetupWorldCanvas();
        SetupButtons();
        HideWorldCanvas();
        
        // Find all existing tiles in the grid
        FindAllGridTiles();
    }

    void Update()
    {
        if (isPlacementMode)
        {
            HandleTilePlacement();
            HandleRotation();
        }
    }

    private void SetupWorldCanvas()
    {
        if (worldCanvasPrefab != null)
        {
            worldCanvasInstance = Instantiate(worldCanvasPrefab);
            worldCanvasInstance.SetActive(false);
        }
    }

    private void SetupButtons()
    {
        if (checkButton != null)
        {
            checkButton.onClick.AddListener(ConfirmPlacement);
        }
        
        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(CancelPlacement);
        }
    }

    private void FindAllGridTiles()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            gridTiles[tile.transform.position] = tile;
        }
    }

    public void StartPlacementMode(Tile tileToPlace)
    {
        if (tileToPlace == null) return;
        
        currentTileToPlace = Instantiate(tileToPlace);
        currentTileToPlace.transform.position = new Vector3(0, 100, 0); // Hide initially
        
        // Set preview material
        if (previewMaterial != null)
        {
            Renderer renderer = currentTileToPlace.GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterial = renderer.material;
                renderer.material = previewMaterial;
            }
        }
        
        isPlacementMode = true;
        ShowWorldCanvas();
    }

    private void HandleTilePlacement()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
        {
            hoveredTile = hit.collider.GetComponent<Tile>();
            
            if (hoveredTile != null)
            {
                placementPosition = hoveredTile.transform.position;
                currentTileToPlace.transform.position = placementPosition;
                
                // Check if placement is valid
                isValidPlacement = ValidatePlacement(placementPosition);
                UpdateTileVisual();
                
                // Update world canvas position
                UpdateWorldCanvasPosition(placementPosition);
                
                // Handle click to select position
                if (Input.GetMouseButtonDown(0))
                {
                    if (isValidPlacement)
                    {
                        selectedTile = hoveredTile;
                    }
                }
            }
        }
    }

    private void HandleRotation()
    {
        if (currentTileToPlace != null)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentTileToPlace.Rotate(-1);
                isValidPlacement = ValidatePlacement(placementPosition);
                UpdateTileVisual();
            }
            else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentTileToPlace.Rotate(1);
                isValidPlacement = ValidatePlacement(placementPosition);
                UpdateTileVisual();
            }
        }
    }

    private bool ValidatePlacement(Vector3 position)
    {
        if (currentTileToPlace == null) return false;
        
        // Check if position already has a tile
        if (gridTiles.ContainsKey(position) && gridTiles[position] != null)
        {
            // Get neighboring tiles and check border compatibility
            return CheckBorderCompatibility(position);
        }
        
        return false;
    }

    private bool CheckBorderCompatibility(Vector3 position)
    {
        // Check all 6 neighbors of hexagonal tile
        Vector3[] neighborOffsets = GetHexNeighborOffsets();
        
        for (int i = 0; i < 6; i++)
        {
            Vector3 neighborPos = position + neighborOffsets[i];
            
            if (gridTiles.ContainsKey(neighborPos))
            {
                Tile neighbor = gridTiles[neighborPos];
                if (neighbor != null)
                {
                    // Get opposite side index
                    int oppositeSide = (i + 3) % 6;
                    
                    // Check if border types match
                    Tile.Type currentBorder = currentTileToPlace.GetSideType(i);
                    Tile.Type neighborBorder = neighbor.GetSideType(oppositeSide);
                    
                    if (currentBorder != neighborBorder)
                    {
                        return false;
                    }
                }
            }
        }
        
        return true;
    }

    private Vector3[] GetHexNeighborOffsets()
    {
        const float sqrt3 = 1.73205080757f;
        float tileSize = 1f; // Should match HexGridGenerator tileSize
        
        return new Vector3[]
        {
            new Vector3(sqrt3 * tileSize, 0, 0),           // Right
            new Vector3(sqrt3/2 * tileSize, 0, 1.5f * tileSize),    // Top-right
            new Vector3(-sqrt3/2 * tileSize, 0, 1.5f * tileSize),   // Top-left
            new Vector3(-sqrt3 * tileSize, 0, 0),          // Left
            new Vector3(-sqrt3/2 * tileSize, 0, -1.5f * tileSize),  // Bottom-left
            new Vector3(sqrt3/2 * tileSize, 0, -1.5f * tileSize)    // Bottom-right
        };
    }

    private void UpdateTileVisual()
    {
        if (currentTileToPlace == null) return;
        
        Renderer renderer = currentTileToPlace.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = isValidPlacement ? validPlacementMaterial : invalidPlacementMaterial;
        }
    }

    private void UpdateWorldCanvasPosition(Vector3 position)
    {
        if (worldCanvasInstance != null)
        {
            worldCanvasInstance.transform.position = position + Vector3.up * 2f;
        }
    }

    private void ShowWorldCanvas()
    {
        if (worldCanvasInstance != null)
        {
            worldCanvasInstance.SetActive(true);
        }
    }

    private void HideWorldCanvas()
    {
        if (worldCanvasInstance != null)
        {
            worldCanvasInstance.SetActive(false);
        }
    }

    public void ConfirmPlacement()
    {
        if (!isValidPlacement || currentTileToPlace == null)
        {
            Debug.Log("Cannot place tile - invalid placement!");
            return;
        }
        
        // Restore original material
        Renderer renderer = currentTileToPlace.GetComponent<Renderer>();
        if (renderer != null && originalMaterial != null)
        {
            renderer.material = originalMaterial;
        }
        
        // Mark tile as chosen
        currentTileToPlace.ChooseTile();
        
        // Update grid dictionary
        gridTiles[placementPosition] = currentTileToPlace;
        
        // Clean up
        EndPlacementMode();
        
        Debug.Log("Tile placed successfully!");
    }

    public void CancelPlacement()
    {
        if (currentTileToPlace != null)
        {
            Destroy(currentTileToPlace.gameObject);
        }
        
        EndPlacementMode();
        
        Debug.Log("Placement cancelled!");
    }

    private void EndPlacementMode()
    {
        isPlacementMode = false;
        currentTileToPlace = null;
        selectedTile = null;
        hoveredTile = null;
        HideWorldCanvas();
    }

    // Public method to get tile at specific position
    public Tile GetTileAtPosition(Vector3 position)
    {
        if (gridTiles.ContainsKey(position))
        {
            return gridTiles[position];
        }
        return null;
    }

    // Public method to check if position is valid for placement
    public bool IsPositionValid(Vector3 position)
    {
        return gridTiles.ContainsKey(position);
    }
}