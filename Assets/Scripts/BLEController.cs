using System.Collections;
using UnityEngine;

public class BLEController : MonoBehaviour
{
    [Header("BLE Device Settings")]
    public string deviceName = "NeuroPlayground Lite";
    public string serviceUUID = "6910123a-eb0d-4c35-9a60-bebe1dcb549d";
    public string charUUID = "5f4f1107-7fc1-43b2-a540-0aa1a9f1ce78";
    
    [Header("Game References")]
    public PlayerController playerController;
    
    [Header("Simulation Settings")]
    public bool enableSimulation = true;
    public float commandInterval = 3.0f; // seconds between commands
    
    // BLE connection status
    private bool isConnected = false;
    private bool isScanning = false;
    
    void Start()
    {
        // Find PlayerController if not assigned
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();
            
        Debug.Log("🔵 BLE Controller initialized");
        Debug.Log("Press 'C' to connect to NeuroPlayground Lite");
        Debug.Log("Press 'D' to disconnect");
    }
    
    void Update()
    {
        // Manual connection controls
        if (Input.GetKeyDown(KeyCode.C) && !isConnected && !isScanning)
        {
            StartBLEConnection();
        }
        
        if (Input.GetKeyDown(KeyCode.D) && isConnected)
        {
            DisconnectBLE();
        }
    }
    
    void StartBLEConnection()
    {
        Debug.Log("🔍 Starting BLE scan for: " + deviceName);
        isScanning = true;
        
        // Start scanning simulation
        StartCoroutine(ScanForDevice());
    }
    
    IEnumerator ScanForDevice()
    {
        Debug.Log("📡 Scanning for NeuroPlayground Lite...");
        
        // Simulate scanning time
        yield return new WaitForSeconds(2.0f);
        
        // Simulate successful connection
        OnDeviceConnected();
    }
    
    void OnDeviceConnected()
    {
        isConnected = true;
        isScanning = false;
        Debug.Log("✅ Connected to NeuroPlayground Lite!");
        Debug.Log($"📋 Service UUID: {serviceUUID}");
        Debug.Log($"📋 Characteristic UUID: {charUUID}");
        
        if (enableSimulation)
        {
            // Start listening for simulated notifications
            StartCoroutine(SimulateESP32Commands());
        }
    }
    
    IEnumerator SimulateESP32Commands()
    {
        Debug.Log("🎯 Starting BLE notification simulation...");
        Debug.Log($"⏱️  Commands will alternate every {commandInterval} seconds");
        
        int currentCommand = 0; // Start with shrink command
        
        while (isConnected)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(commandInterval);
            
            if (!isConnected) break; // Exit if disconnected
            
            // Send the current command
            OnBLECommandReceived(currentCommand);
            
            // Toggle between 0 and 1 (just like your ESP32 firmware)
            currentCommand = currentCommand == 0 ? 1 : 0;
        }
        
        Debug.Log("🔴 BLE notification simulation stopped");
    }
    
    // Called when BLE notification is received (simulated or real)
    void OnBLECommandReceived(int command)
    {
        Debug.Log($"📨 BLE Command Received: {command}");
        
        if (playerController == null)
        {
            Debug.LogError("❌ PlayerController reference is missing!");
            return;
        }
        
        switch (command)
        {
            case 0:
                Debug.Log("🔽 Command 0 → Shrinking player");
                playerController.ShrinkPlayer();
                break;
                
            case 1:
                Debug.Log("🔼 Command 1 → Restoring player to normal size");
                playerController.RestorePlayer();
                break;
                
            default:
                Debug.LogWarning($"⚠️  Unknown command received: {command}");
                break;
        }
    }
    
    void DisconnectBLE()
    {
        isConnected = false;
        Debug.Log("❌ Disconnected from NeuroPlayground Lite");
        Debug.Log("🔵 Press 'C' to reconnect");
    }
    
    // Public methods for external control (useful for UI later)
    public bool IsConnected() { return isConnected; }
    public bool IsScanning() { return isScanning; }
    
    public void ConnectToDevice()
    {
        if (!isConnected && !isScanning)
            StartBLEConnection();
    }
    
    public void Disconnect()
    {
        if (isConnected)
            DisconnectBLE();
    }
    
    // Method for real BLE integration later
    public void ProcessRealBLENotification(byte[] data)
    {
        if (data != null && data.Length > 0)
        {
            int command = data[0]; // Your ESP32 sends single byte (0 or 1)
            OnBLECommandReceived(command);
        }
    }
}
