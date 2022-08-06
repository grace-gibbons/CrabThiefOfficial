using System;

using CrabThief.GameComponents.Audio;
using CrabThief.GameComponents.Entities;
using CrabThief.GameComponents.GUI;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Map.Tiles.Collectibles;
using CrabThief.GameComponents.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

//Shell board
namespace CrabThief.GameComponents.Map.Tiles {
    class ShellBoard : WorldTile {

        //Types of shell boards (empty to put shells in and reference to copy)
        public enum BoardTypes {
            empty,
            reference
        }

        /**
         *   Board Setup:
         *   0  1
         *   2  3 
         *   Shell positions will keep track of where shells are on the board
         */
        private Shell.ShellTypes[] shellPositions; 

        //Path to data file
        private static readonly string EMPTY_BOARD_PATH = "Content/Assets/Components/Tiles/WorldTiles/emptyShellBoard.json";

        //Type of board
        private BoardTypes type;

        //Sound that plays when the board is successfully completed
        private SoundEffect completedBoardAudio;
        //Sound that plays when a shell is placed on the board
        private SoundEffect shellPlacedAudio; 

        /// <summary>
        /// Create shell board
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="type"></param>
        public ShellBoard(Vector2 coordinates, BoardTypes type) {
            //Type will determine what stuff gets loaded below
            this.type = type;

            shellPositions = new Shell.ShellTypes[4];
            shellPositions[0] = Shell.ShellTypes.None;
            shellPositions[1] = Shell.ShellTypes.None;
            shellPositions[2] = Shell.ShellTypes.None;
            shellPositions[3] = Shell.ShellTypes.None;

            ReadWallData(EMPTY_BOARD_PATH);
            //If reference, overwrite image data and shell position data
            if (type == BoardTypes.reference) {
                LoadBoardPath();
            }
           
            SetCoordinates(coordinates);

            //Set position basd on size and grid location
            SetPosition(coordinates * GetWorldTileSize());

            //Set collision body
            SetCollisionBody(new CollisionBody(GetPosition(), GetSize()));
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content) {
            base.LoadContent(content);
            completedBoardAudio = content.Load<SoundEffect>("Assets/Sounds/completedTask");
            shellPlacedAudio = content.Load<SoundEffect>("Assets/Sounds/ShellPlaced");
        }

        /// <summary>
        /// Pick a random configuration for the board
        /// Set the image here, will have same specs as the loaded json file, except for image path, so load that here
        /// </summary>
        public void LoadBoardPath() {
            int rand = new Random().Next(0, 8);
            if(rand == 0) {
                SetTexturePath("Assets/Textures/Tiles/ReferenceShellBoards/referenceShellBoard1");
                //Set the shell positions
                shellPositions[0] = Shell.ShellTypes.Orange; 
                shellPositions[1] = Shell.ShellTypes.Blue; 
                shellPositions[2] = Shell.ShellTypes.Purple; 
                shellPositions[3] = Shell.ShellTypes.Pink; 
            } else if (rand == 1) {
                SetTexturePath("Assets/Textures/Tiles/ReferenceShellBoards/referenceShellBoard2");
                //Set the shell positions
                shellPositions[0] = Shell.ShellTypes.Orange;
                shellPositions[1] = Shell.ShellTypes.Pink;
                shellPositions[2] = Shell.ShellTypes.Purple;
                shellPositions[3] = Shell.ShellTypes.Blue;
            } else if (rand == 2) {
                SetTexturePath("Assets/Textures/Tiles/ReferenceShellBoards/referenceShellBoard3");
                //Set the shell positions
                shellPositions[0] = Shell.ShellTypes.Pink;
                shellPositions[1] = Shell.ShellTypes.Orange;
                shellPositions[2] = Shell.ShellTypes.Blue;
                shellPositions[3] = Shell.ShellTypes.Purple;
            } else if (rand == 3) {
                SetTexturePath("Assets/Textures/Tiles/ReferenceShellBoards/referenceShellBoard4");
                //Set the shell positions
                shellPositions[0] = Shell.ShellTypes.Pink;
                shellPositions[1] = Shell.ShellTypes.Purple;
                shellPositions[2] = Shell.ShellTypes.Blue;
                shellPositions[3] = Shell.ShellTypes.Orange;
            } else if (rand == 4) {
                SetTexturePath("Assets/Textures/Tiles/ReferenceShellBoards/referenceShellBoard5");
                //Set the shell positions
                shellPositions[0] = Shell.ShellTypes.Blue;
                shellPositions[1] = Shell.ShellTypes.Purple;
                shellPositions[2] = Shell.ShellTypes.Pink;
                shellPositions[3] = Shell.ShellTypes.Orange;
            } else if (rand == 5) {
                SetTexturePath("Assets/Textures/Tiles/ReferenceShellBoards/referenceShellBoard6");
                //Set the shell positions
                shellPositions[0] = Shell.ShellTypes.Blue;
                shellPositions[1] = Shell.ShellTypes.Pink;
                shellPositions[2] = Shell.ShellTypes.Orange;
                shellPositions[3] = Shell.ShellTypes.Purple;
            } else if (rand == 6) {
                SetTexturePath("Assets/Textures/Tiles/ReferenceShellBoards/referenceShellBoard7");
                //Set the shell positions
                shellPositions[0] = Shell.ShellTypes.Purple;
                shellPositions[1] = Shell.ShellTypes.Orange;
                shellPositions[2] = Shell.ShellTypes.Blue;
                shellPositions[3] = Shell.ShellTypes.Pink;
            } else {
                SetTexturePath("Assets/Textures/Tiles/ReferenceShellBoards/referenceShellBoard8");
                //Set the shell positions
                shellPositions[0] = Shell.ShellTypes.Purple;
                shellPositions[1] = Shell.ShellTypes.Blue;
                shellPositions[2] = Shell.ShellTypes.Pink;
                shellPositions[3] = Shell.ShellTypes.Orange;
            } 
        }

        /// <summary>
        /// Update the shell board
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <param name="camera"></param>
        /// <param name="player"></param>
        /// <param name="overlayHandler"></param>
        /// <param name="worldMap"></param>
        public void Update(CollisionEngine collisionEngine, GameMouse mouse, GameCamera camera, Player player, OverlayHandler overlayHandler, WorldMap worldMap) {
            //Update the empty board with the new shells, if player tries to place the shells in the board
            if(type == ShellBoard.BoardTypes.empty) {
                //If the distance between the player and the empty board is 3 or less, the mouse is collising with the board, and the mouse has been clicked
                if (collisionEngine.GetDistance(player.GetCoordinates(), GetCoordinates()) <= 3 && collisionEngine.IsMouseCollision(camera, mouse, GetCollisionBody()) && mouse.IsLeftButton()) { 
                    //Get the current selected shell in the gui
                    Shell.ShellTypes currentShell = overlayHandler.GetCurrentShell();

                    //Get the quadrant on the board where the mouse is colliding
                    int collisionArea = collisionEngine.GetAreaCollision(camera, mouse, GetCollisionBody());

                    //Location to put the shell, on the shell board
                    Vector2 newShellLocation = BoardSlotToPosition(collisionArea);

                    //If there is already a shell at that board position, take it out and put it back in the inventory
                    if(shellPositions[collisionArea] != Shell.ShellTypes.None) {
                        //Get the shell in the board that has been clicked
                        Shell.ShellTypes currentBoardShell = shellPositions[collisionArea];
                        //Set that position to empty 
                        shellPositions[collisionArea] = Shell.ShellTypes.None;

                        //TODO Play some "shell removed" sound
                        AudioHandler.PlaySound(shellPlacedAudio); 

                        if (currentBoardShell == Shell.ShellTypes.Orange) {
                            overlayHandler.GetGameplay().GetOrangeShell().SetShowTexture1(true);
                            overlayHandler.GetGameplay().GetOrangeShell().SetShowTexture0(false);

                            worldMap.GetOrangeShell().SetIsPlaced(false);
                        } else if (currentBoardShell == Shell.ShellTypes.Blue) {
                            overlayHandler.GetGameplay().GetBlueShell().SetShowTexture1(true);
                            overlayHandler.GetGameplay().GetBlueShell().SetShowTexture0(false);

                            worldMap.GetBlueShell().SetIsPlaced(false);
                        } else if (currentBoardShell == Shell.ShellTypes.Pink) {
                            overlayHandler.GetGameplay().GetPinkShell().SetShowTexture1(true);
                            overlayHandler.GetGameplay().GetPinkShell().SetShowTexture0(false);

                            worldMap.GetPinkShell().SetIsPlaced(false);
                        } else if (currentBoardShell == Shell.ShellTypes.Purple) {
                            overlayHandler.GetGameplay().GetPurpleShell().SetShowTexture1(true);
                            overlayHandler.GetGameplay().GetPurpleShell().SetShowTexture0(false);

                            worldMap.GetPurpleShell().SetIsPlaced(false); 
                        }

                    } else {
                        //Set the current shell to none, remove it from the gui, and set the image to the proper location inside the shell board
                        //Make one if a method, then pass the shell in??
                        overlayHandler.SetCurrentShell(Shell.ShellTypes.None);

                        //TODO Play some "shell placed" sound
                        AudioHandler.PlaySound(shellPlacedAudio);

                        if (currentShell == Shell.ShellTypes.Orange) {
                            overlayHandler.GetGameplay().GetOrangeShell().SetShowTexture1(false);
                            overlayHandler.GetGameplay().GetOrangeShell().SetShowTexture0(true);

                            worldMap.GetOrangeShell().SetIsPlaced(true);
                            worldMap.GetOrangeShell().SetPosition(newShellLocation);

                            shellPositions[collisionArea] = Shell.ShellTypes.Orange;

                        } else if (currentShell == Shell.ShellTypes.Blue) {
                            overlayHandler.GetGameplay().GetBlueShell().SetShowTexture1(false);
                            overlayHandler.GetGameplay().GetBlueShell().SetShowTexture0(true);

                            worldMap.GetBlueShell().SetIsPlaced(true);
                            worldMap.GetBlueShell().SetPosition(newShellLocation);

                            shellPositions[collisionArea] = Shell.ShellTypes.Blue;

                        } else if (currentShell == Shell.ShellTypes.Pink) {
                            overlayHandler.GetGameplay().GetPinkShell().SetShowTexture1(false);
                            overlayHandler.GetGameplay().GetPinkShell().SetShowTexture0(true);

                            worldMap.GetPinkShell().SetIsPlaced(true);
                            worldMap.GetPinkShell().SetPosition(newShellLocation);

                            shellPositions[collisionArea] = Shell.ShellTypes.Pink;

                        } else if (currentShell == Shell.ShellTypes.Purple) {
                            overlayHandler.GetGameplay().GetPurpleShell().SetShowTexture1(false);
                            overlayHandler.GetGameplay().GetPurpleShell().SetShowTexture0(true);

                            worldMap.GetPurpleShell().SetIsPlaced(true);
                            worldMap.GetPurpleShell().SetPosition(newShellLocation);

                            shellPositions[collisionArea] = Shell.ShellTypes.Purple;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Convert the spot on the shell board where the mouse is clicking (0, 1, 2, 3) to a position for the placed shell to be drawn
        /// </summary>
        /// <param name="collisionArea"></param>
        /// <returns></returns>
        public Vector2 BoardSlotToPosition(int collisionArea) {
            Vector2 topRightCorner = GetPosition() + new Vector2(7, 7);
            //Update the shell board image with the new shell - may need to wait until the images are sized properly
            if (collisionArea == 0) {
                //Console.WriteLine("Put " + currentShell + " in area 0");
                return topRightCorner;
            } else if (collisionArea == 1) {
                //Console.WriteLine("Put " + currentShell + " in area 1");
                return new Vector2(GetPosition().X + (GetSize().X / 2) + 1, GetPosition().Y + 7);
            } else if (collisionArea == 2) {
                //Console.WriteLine("Put " + currentShell + " in area 2");
                return new Vector2(GetPosition().X + 7, GetPosition().Y + (GetSize().Y / 2) + 1);
            } else if (collisionArea == 3) {
                //Console.WriteLine("Put " + currentShell + " in area 3");
                return new Vector2(GetPosition().X + (GetSize().X / 2) + 1, GetPosition().Y + (GetSize().Y / 2) + 1);
            }
            return Vector2.Zero; 
        }

        /// <summary>
        /// Return true if the shell boards are equal (all shell placements match)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ShellBoard) { 
                ShellBoard t = (ShellBoard)obj;
                if(this.shellPositions[0] == t.shellPositions[0] && this.shellPositions[1] == t.shellPositions[1] && this.shellPositions[2] == t.shellPositions[2] && this.shellPositions[3] == t.shellPositions[3]) {
                    return true; 
                }
            }
            return false;
        }

        public SoundEffect GetCompletedBoardAudio() {
            return completedBoardAudio;
        }
    }
}
