using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputBuffer : MonoBehaviour
{
    public struct BufferedCommand
    {
        public string Command;
        public float Timestamp;

        public BufferedCommand(string command, float timestamp)
        {
            Command = command; Timestamp = timestamp;
        }
    }

    [Header("Buffer Configuration")]
    [Tooltip("How long an input stays valid in the queue (e.g., 0.2s = ~12 frames at 60fps)")]
    [SerializeField] private float BufferWindow = 0.22f;
    [SerializeField] private float DoubleTapWindow = 0.25f;

    [Header("Input References")]
    [SerializeField] private InputActionReference MoveAction;

    private readonly List<BufferedCommand> buffer = new List<BufferedCommand>();

    // Internal motion parsing
    private float LastTapTime = -1f;
    private int LastTapDirection = 0;
    private int PreviousXInput = 0;

    private void Update()
    {
        CleanExpiredInputs();
        ParseDasheInputs();
    }

    private void CleanExpiredInputs()
    {
        for (int i = buffer.Count - 1; i >= 0; i--)
        {
            if (Time.time - buffer[i].Timestamp > BufferWindow)
            {
                buffer.RemoveAt(i);
            }
        }
    }

    private void ParseDasheInputs()
    {
        float RawX = MoveAction.action.ReadValue<Vector2>().x;
        int CurrentXInput = 0;

        if (RawX > 0.4f) CurrentXInput = 1;
        else if (RawX < -0.4f) CurrentXInput = -1;

        // Detect clean neutral-to-direction transition
        if (CurrentXInput != 0 && PreviousXInput == 0)
        {
            if (CurrentXInput == LastTapDirection && Time.time - LastTapTime <= DoubleTapWindow)
            {
                // Double tap detected. Buffer the high-level command
                string DashCommand = CurrentXInput == 1 ? "DashRight" : "DashLeft";
                AddInput(DashCommand);
                LastTapDirection = 0;
            } else
            {
                LastTapDirection = CurrentXInput;
                LastTapTime = Time.time;
            }
        }

        PreviousXInput = CurrentXInput;

    }

    /// <summary>
    /// Manually push an input into the buffer (used for attacks, special moves, etc.)
    /// </summary>
    public void AddInput(string command)
    {
        // Remove existing identical commands to prevent duplicate triggers
        buffer.RemoveAll(c => c.Command == command);
        buffer.Add(new BufferedCommand(command, Time.time));
    }

    /// <summary>
    /// Checks if a command is in the buffer. If found, consumes it so it can't fire twice.
    /// </summary>
    public bool TryConsume(string command)
    {
        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].Command == command)
            {
                buffer.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Clears all pending inputs (useful on getting hit or dying).
    /// </summary>
    public void Clear()
    {
        buffer.Clear();
    }
}
