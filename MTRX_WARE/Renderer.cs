﻿using ImGuiNET;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ClickableTransparentOverlay;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace MTRX_WARE
{
    public class Renderer : Overlay
    {
        // render variables
        public Vector2 screenSize = new Vector2(1920, 1080); // own screen size

        // entities copy | more thread safe method
        private ConcurrentQueue<Entity> entities = new ConcurrentQueue<Entity>();
        private Entity localPlayer = new Entity();
        private readonly object entityLock = new object();

        // Gui elements
        private bool enableESP = true;
        private bool enableNameESP = true;
        private bool enableBhop = false;
        private bool enableLineESP = true;
        private bool enableBoxESP = true;
        private bool enableSkeletonESP = true;
        private bool enableDistanceESP = true;
        private Vector4 enemyColor = new Vector4(1, 0, 0, 1); // default red
        private Vector4 teamColor = new Vector4(0, 1, 0, 1); // default green
        private Vector4 nameColor = new Vector4(1, 1, 1, 1); // default white
        private Vector4 boneColor = new Vector4(1, 1, 1, 1); // default white

        float boneThickness = 4;

        // draw list
        ImDrawListPtr drawlist;

        private int switchTabs = 0;
        protected override void Render()
        {
            ImGui.Begin("MTRX-Ware.io");
            // style colors
            ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.2f, 0.2f, 0.2f, 0.8f));

            ImGui.PushStyleColor(ImGuiCol.TitleBg, new Vector4(0.15f, 0.15f, 0.15f, 1.0f));
            ImGui.PushStyleColor(ImGuiCol.TitleBgActive, new Vector4(0.15f, 0.15f, 0.15f, 1.0f));

            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.18f, 0.18f, 0.18f, 1.0f));
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0.25f, 0.25f, 0.25f, 1.0f));
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0.22f, 0.22f, 0.22f, 1.0f));

            ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.18f, 0.18f, 0.18f, 1.0f));       // Checkbox background
            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, new Vector4(0.25f, 0.25f, 0.25f, 1.0f)); // Hover state for checkbox
            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, new Vector4(0.5f, 0.5f, 0.5f, 1.0f));  // Active state for checkbox
            ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(0.8f, 0.8f, 0.8f, 1.0f));  // Checkmark color

            ImGui.PushStyleColor(ImGuiCol.Header, new Vector4(0.18f, 0.18f, 0.18f, 1.0f));        // Default color for header
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new Vector4(0.25f, 0.25f, 0.25f, 1.0f)); // Hovered state for header
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, new Vector4(0.22f, 0.22f, 0.22f, 1.0f));  // Active state for header

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(10, 10));
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(10, 5));

            // Menu buttons
            if (ImGui.Button("Aimbot", new Vector2(100.0f, 0.0f)))
                switchTabs = 0;
            ImGui.SameLine(0.0f, 2.0f); // Same line with small spacing
            if (ImGui.Button("Visuals", new Vector2(100.0f, 0.0f)))
                switchTabs = 1;
            ImGui.SameLine(0.0f, 2.0f); // Same line with small spacing
            if (ImGui.Button("Exploits", new Vector2(100.0f, 0.0f)))
                switchTabs = 2;
            ImGui.SameLine(0.0f, 2.0f); // Same line with small spacing
            if (ImGui.Button("Misc", new Vector2(100.0f, 0.0f)))
                switchTabs = 3;

            float windowWidth = ImGui.GetWindowWidth();
            float lineY = ImGui.GetItemRectMax().Y + 10; // Position the line slightly below the buttons

            // Draw a line spanning the window width
            ImGui.GetWindowDrawList().AddLine(
                new Vector2(ImGui.GetWindowPos().X, lineY),
                new Vector2(ImGui.GetWindowPos().X + windowWidth, lineY),
                ImGui.ColorConvertFloat4ToU32(new Vector4(0.15f, 0.15f, 0.15f, 1.0f)), // Darker than background
                2.0f // Line thickness
            );

            switch (switchTabs)
            {
                case 0: // AIMBOT
                    
                    break;

                case 1: // VISUALS
                    ImGui.Checkbox("Enable ESP", ref enableESP);
                    ImGui.Checkbox("Enable Box ESP", ref enableBoxESP);
                    ImGui.Checkbox("Enable Skeleton ESP", ref enableSkeletonESP);
                    ImGui.Checkbox("Enable Line ESP", ref enableLineESP);
                    ImGui.Checkbox("Enable Name ESP", ref enableNameESP);
                    ImGui.Checkbox("Enable Distance ESP", ref enableDistanceESP);
                    // team color
                    ImGui.Text("Team Color");
                    ImGui.SameLine();
                    ImGui.ColorEdit4("##teamcolor", ref teamColor, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.NoLabel | ImGuiColorEditFlags.AlphaPreviewHalf);
                    // enemy color
                    ImGui.Text("Enemy Color");
                    ImGui.SameLine();
                    ImGui.ColorEdit4("##enemycolor", ref enemyColor, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.NoLabel | ImGuiColorEditFlags.AlphaPreviewHalf);
                    // bone color
                    // enemy color
                    ImGui.Text("Bone Color");
                    ImGui.SameLine();
                    ImGui.ColorEdit4("##bonecolor", ref boneColor, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.NoLabel | ImGuiColorEditFlags.AlphaPreviewHalf);
                    break;

                case 2: // Exploits
                    ImGui.Checkbox("Enable Teleport", ref enableBhop);
                    ImGui.Checkbox("Enable Flip", ref enableBhop);
                    ImGui.Checkbox("Enable Desync", ref enableBhop);
                    break;
                case 3:
                    ImGui.Checkbox("Enable Bhop", ref enableBhop);
                    break;
            }

            // draw overlay
            DrawOverlay(screenSize);
            drawlist = ImGui.GetWindowDrawList();

            // draw stuff
            if (enableESP)
            {
                if (enableBoxESP)
                {

                    foreach(var entity in entities)
                    {
                        if (EntityOnScreen(entity))
                        {
                                DrawBox(entity);
                        }
                    }
                }
                if (enableLineESP)
                {
                    foreach (var entity in entities)
                    {
                        if (EntityOnScreen(entity))
                        {
                            DrawLine(entity);
                        }
                    }
                }
                if (enableNameESP)
                {
                    foreach (var entity in entities)
                    {
                        if (EntityOnScreen(entity))
                        {
                            DrawName(entity, 20);
                        }
                    }
                }
                if (enableSkeletonESP)
                {
                    foreach (var entity in entities)
                    {
                        if (EntityOnScreen(entity))
                        {
                            DrawBones(entity);
                        }
                    }
                }
                if (enableDistanceESP)
                {
                    foreach (var entity in entities)
                    {
                        if (EntityOnScreen(entity))
                        {
                            DrawDistance(entity, 65);
                        }
                    }
                }
            }
        }

        // check position
        bool EntityOnScreen(Entity entity)
        {
            if (entity.position2D.X > 0 && entity.position2D.X < screenSize.X && entity.position2D.Y > 0 && entity.position2D.Y < screenSize.Y)
            {
                return true;
            }
            return false;
        }

        // drawing method

        private void DrawBones(Entity entity)
        {
            uint uintColor = ImGui.ColorConvertFloat4ToU32(boneColor);
            float currentBoneThickness = boneThickness / entity.distance;

            drawlist.AddLine(entity.bones2d[1], entity.bones2d[2], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[1], entity.bones2d[3], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[1], entity.bones2d[6], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[3], entity.bones2d[4], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[6], entity.bones2d[7], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[4], entity.bones2d[5], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[7], entity.bones2d[8], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[1], entity.bones2d[0], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[0], entity.bones2d[9], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[0], entity.bones2d[11], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[9], entity.bones2d[10], uintColor, currentBoneThickness);
            drawlist.AddLine(entity.bones2d[11], entity.bones2d[12], uintColor, currentBoneThickness);
            drawlist.AddCircle(entity.bones2d[1], 5 + currentBoneThickness, uintColor);
        }

        public void DrawName(Entity entity, int yOffset)
        {
            Vector2 textLocation = new Vector2(entity.viewPosition2D.X, entity.viewPosition2D.Y - yOffset);
            drawlist.AddText(textLocation, ImGui.ColorConvertFloat4ToU32(nameColor), $"{entity.name}");
        }

        public void DrawDistance(Entity entity, int yOffset2)
        {
            Vector2 textLocation = new Vector2(entity.viewPosition2D.X, entity.viewPosition2D.Y + yOffset2);
            drawlist.AddText(textLocation, ImGui.ColorConvertFloat4ToU32(nameColor), $"{entity.distance}");
        }

        public void DrawBox(Entity entity)
        {
            // box height
            float entityHeight = entity.position2D.Y - entity.viewPosition2D.Y;

            // box dimensions
            Vector2 rectTop = new Vector2(entity.viewPosition2D.X - entityHeight / 3, entity.viewPosition2D.Y);

            Vector2 rectBottom = new Vector2(entity.viewPosition2D.X + entityHeight / 3, entity.position2D.Y);

            // get correct color
            Vector4 boxColor = localPlayer.team == entity.team ? teamColor : enemyColor;

            drawlist.AddRect(rectTop, rectBottom, ImGui.ColorConvertFloat4ToU32(boxColor));
        }
        private void DrawLine(Entity entity)
        {   
            // get correct team color
            Vector4 lineColor = localPlayer.team == entity.team ? teamColor: enemyColor;

            // draw line
            drawlist.AddLine(new Vector2(screenSize.X / 2, screenSize.Y), entity.position2D, ImGui.ColorConvertFloat4ToU32(lineColor));
        }

        // transfer entity methods

        public void UpdateEntities(IEnumerable<Entity> newEntities)
        {
            entities = new ConcurrentQueue<Entity>(newEntities);
        }

        public void UpdateLocalPlayer(Entity newEntity)
        {
            lock(entityLock)
            {
                localPlayer = newEntity;
            }
        }

        public Entity GetLocalPlayer()
        {
            lock(entityLock) {
            return localPlayer;
            }
        }

        void DrawOverlay(Vector2 screenSize) // Overlay window
        {
            ImGui.SetNextWindowSize(screenSize);
            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.Begin("overlay", ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoInputs
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoScrollWithMouse
                );
        }
    }
}
