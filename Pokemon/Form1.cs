﻿//20th June 2024
//Tri's and Ayush Patel's project
//Pokemon Game
//ISC3U Mr.T
using Pokemon.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;
using System.IO;

namespace Pokemon
{


    public partial class MainForm : Form
    {
        //Sound properties
        System.Windows.Media.MediaPlayer normalAtkSound;
        System.Windows.Media.MediaPlayer chestCollectSound;
        System.Windows.Media.MediaPlayer grassStepSound = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer healSound;
        System.Windows.Media.MediaPlayer levelUpSound;
        System.Windows.Media.MediaPlayer openDoorSound;
        System.Windows.Media.MediaPlayer spAtkSound;
        System.Windows.Media.MediaPlayer treeHitSound;
        System.Windows.Media.MediaPlayer battleMusic;


        //Player
        Rectangle player1 = new Rectangle(400, 250, 35, 40);

        //Player Assets
        Image[] playerDownImages = { Properties.Resources.down1, Properties.Resources.down2, Properties.Resources.down3, Properties.Resources.down4 };
        Image[] playerLeftImages = { Properties.Resources.left1, Properties.Resources.left2, Properties.Resources.left3, Properties.Resources.left4 };
        Image[] playerRightImages = { Properties.Resources.right1, Properties.Resources.right2, Properties.Resources.right3, Properties.Resources.right4 };
        Image[] playerUpImages = { Properties.Resources.up1, Properties.Resources.up2, Properties.Resources.up3, Properties.Resources.up4 };

        //Track Player Direction
        enum Direction { Down, Left, Right, Up }
        Direction playerDirection = Direction.Down; //default direction
        Image playerCurrentDirection;

        //move Animation variables
        int animationFrame = 0;
        int animationSpeed = 2;
        int animationCounter = 0;
        int player1FrameIndex = 0;

        //Random
        Random randGen = new Random();

        //Lists
        List<Rectangle> treesList = new List<Rectangle>();
        List<Rectangle> treesHitBoxList = new List<Rectangle>();
        List<Rectangle> chestList = new List<Rectangle>();

        //Money Lists
        int[] moneyRequired = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200,210, 0 };
        int i = 0;

        //trees Images
        Image[] treesImg = { Properties.Resources.tree1, Properties.Resources.tree2, Properties.Resources.tree3, Properties.Resources.grass };
        List<Image> treeImgList = new List<Image>();

        //Player House Setup
        Image playerHouse = Properties.Resources.house;
        Image HouseInteriorImg = Properties.Resources.houseInterior;
        Image vendingMachineImg = Properties.Resources.evolveMachine;

        //images
        Image[] pokemonsList = { Properties.Resources.pikachu, Properties.Resources.squirtle, Properties.Resources.charmander, Properties.Resources.bulbasaur };
        Image grassImg = Properties.Resources.bg;
        Image forrestImg = Properties.Resources.forest;
        Image chestImg = Properties.Resources.chestImg;
        Image plantArena = Properties.Resources.arena_plant;
        Image UpgradeStatsBg = Properties.Resources.UpgradeStatsBg;
        Image boss1 = Properties.Resources.boss1;
        Image boss2 = Properties.Resources.boss2;
        Image boss3 = Properties.Resources.boss3;
        Image playerPokemon;
        Image winImg = Properties.Resources.winnerImg;

        Image evolveScreenBg = Properties.Resources.evolveScreen;


        //Rectangles
        Rectangle actualChest = new Rectangle(20, 50, 30, 20);
        Rectangle battleBossHealthBar = new Rectangle();
        Rectangle battlePokemonHealthBar = new Rectangle();
        Rectangle house = new Rectangle(480, 100, 230, 200);
        Rectangle door = new Rectangle(578, 290, 35, 10);
        Rectangle houseInterior = new Rectangle(200, -20, 400, 500);
        Rectangle exitDoor = new Rectangle(310, 450, 45, 10);
        Rectangle vendingMachine = new Rectangle(250, 100, 40, 70);
        Rectangle houseWall = new Rectangle(500, 240, 200, 58);
        Rectangle houseWallInterior1 = new Rectangle(220, -20, 10, 500);
        Rectangle houseWallInterior2 = new Rectangle(190, 100, 400, 10);
        Rectangle houseWallInterior3 = new Rectangle(570, -20, 10, 500);
        Rectangle houseWallInterior4 = new Rectangle(200, 440, 110, 10);
        Rectangle houseWallInterior5 = new Rectangle(355, 440, 215, 10);
        Rectangle arenaWall = new Rectangle(295, 147, 200, 150);


        //Grass and Arena
        Rectangle grass = new Rectangle(0, 0, 1000, 600);
        Rectangle grassArena = new Rectangle(300, 100, 200, 200);

        //Arena setup
        Rectangle enterArena = new Rectangle(387, 263, 29, 38);
        Rectangle statsBg = new Rectangle(480, 100, 300, 150);

        //Evolve Pokemon Pic Position
        Rectangle pokemonPicEvolve = new Rectangle(80, 80, 180, 180);
        String actualPokemon;



        //Player Stats
        string playerName;
        string gameState = "Start Screen";

        int pokemonLv = 1;
        int pokemonHealth = 100;
        int pokemonAtk = 60;
        int pokemonSpAtk = 120;
        int pokemonHeal = 40;

        int bossHealth = 1000;
        int bossAtk = 320;
        int bossSpAtk = 500;
        int bossHeal = 600;
        int bossCounter = 1;

        int playerMoney = 100;
        int bossCurrentHealthX = 370;

        //Setup for Battle
        string playerChoice, cpuChoice;
        int choicePause = 1000;
        int outcomePause = 3000;
        int playerTurnCount = 0;
        int cpuTurnCount = 0;
        bool playerSpAttackOnCooldown = false;
        bool cpuSpAttackOnCooldown = false;


        //Brush
        SolidBrush transparent = new SolidBrush(Color.Transparent);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        Font drawFont = new Font("Arial", 16, FontStyle.Bold);

        //Player Speed
        int player1Speed = 10;


        //Player Movement Booleans
        bool wPressed = false;
        bool sPressed = false;
        bool aPressed = false;
        bool dPressed = false;

        public MainForm()
        {
            InitializeComponent();
            //Menu Screen
            InitializeGameScreen();
            battleBossHealthBar = new Rectangle(350, 100, bossHealth / 5, 20);
            battlePokemonHealthBar = new Rectangle(200, 300, pokemonHealth / 5, 20);
            playerCurrentDirection = Properties.Resources.down1;
            evolveButton.Visible = false;
            exitEvolve.Visible = false;
            gameLoop.Enabled = false;
        }


        //PLayer movement keydown
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.A:
                    aPressed = true;
                    break;
                case Keys.D:
                    dPressed = true;
                    break;
                case Keys.Space:
                    if (gameState == "End Screen")
                    {
                        InitializeMenuScreen();
                        gameState = "Start Screen";
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "End Screen")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        //player movement keydown
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.A:
                    aPressed = false;
                    break;
                case Keys.D:
                    dPressed = false;
                    break;
            }
        }

        //player movement
        public void PlayerMovement()
        {
            Rectangle newPlayerPosition = player1;

            //Track if the player is moving
            bool isMoving = false;

            // Diagonal speed normalization
            double speedModifier = 1.0;
            if ((wPressed || sPressed) && (aPressed || dPressed))
            {
                speedModifier = Math.Sqrt(2) / 2.0;
            }

            if (wPressed && newPlayerPosition.Y > 0)
            {
                playGrassSound();
                newPlayerPosition.Y -= player1Speed;
                isMoving = true;
                playerDirection = Direction.Up; //Update direction
            }
            if (sPressed && newPlayerPosition.Y < this.Height - 80)
            {
                playGrassSound();
                newPlayerPosition.Y += player1Speed;
                isMoving = true;
                playerDirection = Direction.Down;
            }
            if (aPressed && newPlayerPosition.X > 0)
            {
                playGrassSound();

                newPlayerPosition.X -= player1Speed;

                if (player1.X < player1.Width && gameState == "Arena Screen")
                {
                    newPlayerPosition.X = this.Width - player1.Width;
                    InitializeMainScreen();

                }
                isMoving = true;
                playerDirection = Direction.Left;

            }
            if (dPressed)
            {
                playGrassSound();
                newPlayerPosition.X += player1Speed;
                isMoving = true;
                playerDirection = Direction.Right;

                if (player1.X > this.Width && gameState == "Main Screen")
                {
                    gameState = "Arena Screen";
                    newPlayerPosition.X = 0;
                }

                if (dPressed == true && gameState == "Arena Screen" && newPlayerPosition.X > this.Width - player1.Width)
                {
                    newPlayerPosition.X = this.Width - player1.Width;
                }
            }

            player1 = newPlayerPosition;
            // Handle animation speed
            if (isMoving)
            {
                animationCounter++;
                if (animationCounter >= animationSpeed)
                {
                    animationCounter = 0;
                    player1FrameIndex = (player1FrameIndex + 1) % GetCurrentDirectionImages().Length;
                }
            }
            else
            {
                player1FrameIndex = 3; // Display the fourth image when not moving
            }
        }


        //player moving images
        private Image[] GetCurrentDirectionImages()
        {
            switch (playerDirection)
            {
                case Direction.Up:
                    return playerUpImages;
                case Direction.Down:
                    return playerDownImages;
                case Direction.Left:
                    return playerLeftImages;
                case Direction.Right:
                    return playerRightImages;
                default:
                    return playerDownImages;
            }
        }


        // draw the hitbox trees in the map
        private void obstacleHitBoxSpawn()
        {
            Rectangle treeHitBox1 = new Rectangle(48, 119, 45, 27);
            treesHitBoxList.Add(treeHitBox1);
            Rectangle treeHitBox2 = new Rectangle(218, 108, 45, 27);
            treesHitBoxList.Add(treeHitBox2);
            Rectangle treeHitBox3 = new Rectangle(518, 83, 45, 27);
            treesHitBoxList.Add(treeHitBox3);
            Rectangle treeHitBox4 = new Rectangle(718, 98, 45, 27);
            treesHitBoxList.Add(treeHitBox4);
            Rectangle treeHitBox5 = new Rectangle(118, 423, 45, 27);
            treesHitBoxList.Add(treeHitBox5);
            Rectangle treeHitBox6 = new Rectangle(318, 273, 45, 27);
            treesHitBoxList.Add(treeHitBox6);
            Rectangle treeHitBox7 = new Rectangle(618, 443, 45, 27);
            treesHitBoxList.Add(treeHitBox7);
            Rectangle treeHitBox8 = new Rectangle(468, 393, 45, 27);
            treesHitBoxList.Add(treeHitBox8);
            Rectangle treeHitBox9 = new Rectangle(98, 263, 45, 27);
            treesHitBoxList.Add(treeHitBox9);
        }

        // draw the trees in the map
        private void obstacleSpawn()
        {
            Rectangle tree1 = new Rectangle(32, 50, 75, 100);
            treesList.Add(tree1);
            Rectangle tree2 = new Rectangle(200, 35, 75, 100);
            treesList.Add(tree2);
            Rectangle tree3 = new Rectangle(500, 15, 75, 100);
            treesList.Add(tree3);
            Rectangle tree4 = new Rectangle(700, 25, 75, 100);
            treesList.Add(tree4);
            Rectangle tree5 = new Rectangle(100, 350, 75, 100);
            treesList.Add(tree5);
            Rectangle tree6 = new Rectangle(300, 200, 75, 100);
            treesList.Add(tree6);
            Rectangle tree7 = new Rectangle(600, 370, 75, 100);
            treesList.Add(tree7);
            Rectangle tree8 = new Rectangle(450, 320, 75, 100);
            treesList.Add(tree8);
            Rectangle tree9 = new Rectangle(80, 190, 75, 100);
            treesList.Add(tree9);

            for (int i = 0; i < treesList.Count; i++)
            {
                if (i % 3 == 0)
                {
                    treeImgList.Add(Properties.Resources.tree1);
                }

                else if (i % 3 == 1)
                {
                    treeImgList.Add(Properties.Resources.tree2);
                }

                else
                {
                    treeImgList.Add(Properties.Resources.tree3);
                }
            }
        }

        //stop the player from going through the trees
        private void obstacleCollision()
        {
            if (gameState == "Main Screen")
            {
                for (int i = 0; i < treesHitBoxList.Count; i++)
                {
                    if (player1.IntersectsWith(treesHitBoxList[i]))
                    {
                        playTreeHitSound();
                        if (wPressed == true)
                        {
                            player1.Y += player1Speed;
                        }
                        if (sPressed == true)
                        {
                            player1.Y -= player1Speed;
                        }
                        if (aPressed == true)
                        {
                            player1.X += player1Speed;
                        }
                        if (dPressed == true)
                        {
                            player1.X -= player1Speed;
                        }
                    }
                }
            }
        }

        //exit the program when 
        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //startButton event, move to the menu screen.
        private void startButton_Click(object sender, EventArgs e)
        {
            InitializePokemonChoosingScreen();
            gameState = "Choosing Screen";
            startButton.Visible = false;
            exitButton.Visible = false;
        }

        //POKEMON SELECTION
        private void pikachu_Click(object sender, EventArgs e)
        {
            pokemonPreview.Image = pokemonsList[0];
        }

        private void charmander_Click(object sender, EventArgs e)
        {
            pokemonPreview.Image = pokemonsList[2];
        }

        private void bulbasaur_Click(object sender, EventArgs e)
        {
            pokemonPreview.Image = pokemonsList[3];
        }

        private void squirtle_Click(object sender, EventArgs e)
        {
            pokemonPreview.Image = pokemonsList[1];
        }

        //continue to the main game screen
        private void continueButton_Click(object sender, EventArgs e)
        {
            InitializeMainScreen();
            InitializeGameScreen();
            gameLoop.Enabled = true;
            chestSpawn();

            playerName = playerNameInput.Text;
            if (pokemonPreview.Image == pokemonsList[0])
            {
                actualPokemon = "Pikachu";
                playerPokemon = pokemonsList[0];
            }
            else if (pokemonPreview.Image == pokemonsList[1])
            {
                actualPokemon = "Squirtle";
                playerPokemon = pokemonsList[1];
            }
            else if (pokemonPreview.Image == pokemonsList[2])
            {
                actualPokemon = "Charmander";
                playerPokemon = pokemonsList[2];

            }
            else if (pokemonPreview.Image == pokemonsList[3])
            {
                actualPokemon = "Bulbasaur";
                playerPokemon = pokemonsList[3];

            }
            this.Focus();
        }

        //reset the player's stats when the game is replay
        public void InitializeMenuScreen()
        {
            gameState = "Start Screen";
            pokemonLv = 0;
            i = 1;
            evolveButton.Enabled = true;
            pokemonLvChecking();
            pokemonHealth = 120;
            pokemonAtk = 130;
            pokemonSpAtk = 150;
            pokemonHeal = 160;
            playerMoney = 0;
            exitButton.Visible = true;
            playAgainLabel.Visible = false;
            startButton.Visible = true;
            battlePokemon.Visible = false;
            bossHealthLabel.Visible = false;
            pokemonHealthLabel.Visible = false;
            pTurn.Visible = false; 
            resultBattleLabel.Visible = false;
            this.BackgroundImage = Properties.Resources.forest;
        }

        public void InitializeGameScreen()
        {
            evolveButton.Visible = false;
            exitEvolve.Visible = false;
            resultBattleLabel.Visible = false;

            pikachu.Visible = false;
            squirtle.Visible = false;
            charmander.Visible = false;
            bulbasaur.Visible = false;
            playerPreview.Visible = false;
            pokemonPreview.Visible = false;
            continueButton.Visible = false;
            characterLabel.Visible = false;
            playerNameInput.Visible = false;
            attackButton.Visible = false;
            healButton.Visible = false;
            spAttackButton.Visible = false;
            battleBoss.Visible = false;
            battlePokemon.Visible = false;
        }

        //hide all the buttons and label when player is in the main game screen
        public void InitializeMainScreen()
        {
            resultBattleLabel.Visible = false;
            attackButton.Visible = false;
            attackButton.Enabled = false;
            healButton.Visible = false;
            healButton.Enabled = false;
            spAttackButton.Visible = false;
            spAttackButton.Enabled = false;

            battleBoss.Visible = false;
            battlePokemon.Visible = false;
            pokemonHealthLabel.Visible = false;
            bossHealthLabel.Visible = false;
            pTurn.Visible = false;
            cooldownNotice.Visible = false;
            gameState = "Main Screen";
            exitButton.Visible = false;
            playAgainLabel.Visible = false;
            startButton.Visible = false;


            this.BackgroundImage = Properties.Resources.bg;
            obstacleSpawn();
            doorCollision();
            obstacleHitBoxSpawn();
            chestSpawn();

            player1.X = 578;
            player1.Y = 300;

            gameLoop.Enabled = true;
            this.Focus();
        }

        //all the methods that play sounds
        public void playNormalAtkSound()
        {
            normalAtkSound = new System.Windows.Media.MediaPlayer();
            normalAtkSound.Open(new Uri(Application.StartupPath + "/Resources/attackSound.wav"));
            normalAtkSound.Play();
        }

        public void playChestSound()
        {
            chestCollectSound = new System.Windows.Media.MediaPlayer();
            chestCollectSound.Open(new Uri(Application.StartupPath + "/Resources/coins.wav"));
            chestCollectSound.Play();
        }

        public void playGrassSound()
        {
            grassStepSound = new System.Windows.Media.MediaPlayer();
            grassStepSound.Open(new Uri(Application.StartupPath + "/Resources/grassFootstep.wav"));
            grassStepSound.Play();
        }

        public void playBattleSound()
        {
            battleMusic = new System.Windows.Media.MediaPlayer();
            battleMusic.Open(new Uri(Application.StartupPath + "/Resources/fightinggg.mp3"));
            battleMusic.Play();
        }


        public void playHealSound()
        {
            healSound = new System.Windows.Media.MediaPlayer();
            healSound.Open(new Uri(Application.StartupPath + "/Resources/healSound.wav"));
            healSound.Play();
        }

        public void playLevelUpSound()
        {
            levelUpSound = new System.Windows.Media.MediaPlayer();
            levelUpSound.Open(new Uri(Application.StartupPath + "/Resources/levelUpSound.wav"));
            levelUpSound.Play();
        }

        public void playOpenDoorSound()
        {
            openDoorSound = new System.Windows.Media.MediaPlayer();
            openDoorSound.Open(new Uri(Application.StartupPath + "/Resources/openDoor.wav"));
            openDoorSound.Play();
        }

        public void playSpAtkSound()
        {
            spAtkSound = new System.Windows.Media.MediaPlayer();
            spAtkSound.Open(new Uri(Application.StartupPath + "/Resources/spAttackSound.wav"));
            spAtkSound.Play();
        }

        public void playTreeHitSound()
        {
            treeHitSound = new System.Windows.Media.MediaPlayer();
            treeHitSound.Open(new Uri(Application.StartupPath + "/Resources/tree.wav"));
            treeHitSound.Play();
        }

        public void InitializeEvolveScreen()
        {
            gameState = "Evolve Screen";
            evolveButton.Visible = true;
            exitEvolve.Visible = true;
        }

        public void InitializePokemonChoosingScreen()
        {
            startButton.Visible = false;
            exitButton.Visible = false;
            titleLabel.Visible = false;

            pikachu.Visible = true;
            squirtle.Visible = true;
            charmander.Visible = true;
            bulbasaur.Visible = true;
            playerPreview.Visible = true;
            pokemonPreview.Visible = true;
            continueButton.Visible = true;
            characterLabel.Visible = true;
            playerNameInput.Visible = true;
        }

        //spawn the chest
        private void chestSpawn()
        {
            if (gameState == "Main Screen")
            {
                Rectangle chest1 = new Rectangle(20, 50, 30, 20);
                chestList.Add(chest1);
                Rectangle chest2 = new Rectangle(150, 90, 30, 20);
                chestList.Add(chest2);
                Rectangle chest3 = new Rectangle(400, 100, 30, 20);
                chestList.Add(chest3);
                Rectangle chest4 = new Rectangle(670, 100, 30, 20);
                chestList.Add(chest4);
                Rectangle chest5 = new Rectangle(100, 300, 30, 20);
                chestList.Add(chest5);
                Rectangle chest6 = new Rectangle(100, 150, 30, 20);
                chestList.Add(chest6);
                Rectangle chest7 = new Rectangle(250, 300, 30, 20);
                chestList.Add(chest7);
                Rectangle chest8 = new Rectangle(400, 300, 30, 20);
                chestList.Add(chest8);
                Rectangle chest9 = new Rectangle(700, 300, 30, 20);
                chestList.Add(chest9);
                Rectangle chest10 = new Rectangle(20, 400, 30, 20);
                chestList.Add(chest10);
                Rectangle chest11 = new Rectangle(350, 400, 30, 20);
                chestList.Add(chest11);
                Rectangle chest12 = new Rectangle(500, 430, 30, 20);
                chestList.Add(chest12);
                Rectangle chest13 = new Rectangle(700, 400, 30, 20);
                chestList.Add(chest13);
            }
        }

        //player intersect with the chest
        private void chestCollision()
        {
            int chestRand = randGen.Next(0, chestList.Count);
            int loot = randGen.Next(1, 500);

            if (gameState == "Main Screen" && player1.IntersectsWith(actualChest))
            {
                playChestSound();
                actualChest.X = chestList[chestRand].X;
                actualChest.Y = chestList[chestRand].Y;
                playerMoney += loot;
            }
        }

        //player intersect with the door and it move to the store
        private void doorCollision()
        {
            if (gameState == "Main Screen" && player1.IntersectsWith(door))
            {
                gameState = "House Interior";
                playOpenDoorSound();
                player1.X = 317;
                player1.Y = 380;

            }

            for (int i = 0; i < treesList.Count(); i++)
            {
                if (gameState == "House Interior" || gameState == "Arena Screen")
                {
                    treesList.RemoveAt(i);
                    treesHitBoxList.RemoveAt(i);
                }
            }

            for (int i = 0; i < chestList.Count(); i++)
            {
                if (gameState == "House Interior" || gameState == "Arena Screen")
                {
                    chestList.RemoveAt(i);
                }
            }

            if (gameState == "House Interior" && player1.IntersectsWith(exitDoor))
            {
                player1.X = door.X + 10;
                player1.Y = door.Y + 10;
                InitializeMainScreen();
            }

            if (gameState == "House Interior" && player1.IntersectsWith(vendingMachine))
            {
                gameState = "Evolve Screen";
                InitializeEvolveScreen();
            }
        }

        //a hitbox that stop the player from going through the house
        private void houseCollision()
        {
            if (gameState == "Main Screen" && player1.IntersectsWith(houseWall))
            {
                if (wPressed == true)
                {
                    player1.Y += player1Speed;
                }
                if (sPressed == true)
                {
                    player1.Y -= player1Speed;
                }
                if (aPressed == true)
                {
                    player1.X += player1Speed;
                }
                if (dPressed == true)
                {
                    player1.X -= player1Speed;
                }
            }
        }

        //a hitbox that stop the player from going through the arena
        private void arenaCollision()
        {
            if (gameState == "Arena Screen" && player1.IntersectsWith(arenaWall))
            {
                if (wPressed == true)
                {
                    player1.Y += player1Speed;
                }
                if (sPressed == true)
                {
                    player1.Y -= player1Speed;
                }
                if (aPressed == true)
                {
                    player1.X += player1Speed;
                }
                if (dPressed == true)
                {
                    player1.X -= player1Speed;
                }
            }
        }

        //a hitbox that stop the player from going through the store

        private void InteriorCollision()
        {
            if (gameState == "House Interior" && player1.IntersectsWith(houseWallInterior1))
            {
                if (aPressed == true)
                {
                    player1.X += player1Speed;
                }
            }
            if (gameState == "House Interior" && player1.IntersectsWith(houseWallInterior2))
            {
                if (wPressed == true)
                {
                    player1.Y += player1Speed;
                }
            }
            if (gameState == "House Interior" && player1.IntersectsWith(houseWallInterior3))
            {
                if (dPressed == true)
                {
                    player1.X -= player1Speed;
                }
            }
            if (gameState == "House Interior" && player1.IntersectsWith(houseWallInterior4))
            {
                if (sPressed == true)
                {
                    player1.Y -= player1Speed;
                }
            }
            if (gameState == "House Interior" && player1.IntersectsWith(houseWallInterior5))
            {
                if (sPressed == true)
                {
                    player1.Y -= player1Speed;
                }
            }
        }

        //pokemon's stats for every lv
        private void pokemonLvChecking()
        {
            switch (pokemonLv)
            {
                case 1:
                    pokemonHealth = 120;
                    pokemonAtk = 130;
                    pokemonSpAtk = 150;
                    pokemonHeal = 160;
                    break;
                case 2:
                    pokemonHealth = 132;
                    pokemonAtk = 143;
                    pokemonSpAtk = 165;
                    pokemonHeal = 176;
                    break;
                case 3:
                    pokemonHealth = 145;
                    pokemonAtk = 157;
                    pokemonSpAtk = 181;
                    pokemonHeal = 194;
                    break;
                case 4:
                    pokemonHealth = 160;
                    pokemonAtk = 173;
                    pokemonSpAtk = 199;
                    pokemonHeal = 213;
                    break;
                case 5:
                    pokemonHealth = 176;
                    pokemonAtk = 190;
                    pokemonSpAtk = 219;
                    pokemonHeal = 234;
                    break;
                case 6:
                    pokemonHealth = 193;
                    pokemonAtk = 209;
                    pokemonSpAtk = 241;
                    pokemonHeal = 258;
                    break;
                case 7:
                    pokemonHealth = 212;
                    pokemonAtk = 230;
                    pokemonSpAtk = 265;
                    pokemonHeal = 283;
                    break;
                case 8:
                    pokemonHealth = 233;
                    pokemonAtk = 253;
                    pokemonSpAtk = 291;
                    pokemonHeal = 311;
                    break;
                case 9:
                    pokemonHealth = 256;
                    pokemonAtk = 278;
                    pokemonSpAtk = 320;
                    pokemonHeal = 342;
                    break;
                case 10:
                    pokemonHealth = 282;
                    pokemonAtk = 305;
                    pokemonSpAtk = 352;
                    pokemonHeal = 376;
                    break;
                case 11:
                    pokemonHealth = 310;
                    pokemonAtk = 335;
                    pokemonSpAtk = 387;
                    pokemonHeal = 413;
                    break;
                case 12:
                    pokemonHealth = 341;
                    pokemonAtk = 368;
                    pokemonSpAtk = 425;
                    pokemonHeal = 454;
                    break;
                case 13:
                    pokemonHealth = 360;
                    pokemonAtk = 404;
                    pokemonSpAtk = 467;
                    pokemonHeal = 499;
                    break;
                case 14:
                    pokemonHealth = 412;
                    pokemonAtk = 444;
                    pokemonSpAtk = 514;
                    pokemonHeal = 549;
                    break;
                case 15:
                    pokemonHealth = 453;
                    pokemonAtk = 488;
                    pokemonSpAtk = 565;
                    pokemonHeal = 603;
                    break;
                case 16:
                    pokemonHealth = 498;
                    pokemonAtk = 537;
                    pokemonSpAtk = 622;
                    pokemonHeal = 663;
                    break;
                case 17:
                    pokemonHealth = 547;
                    pokemonAtk = 590;
                    pokemonSpAtk = 684;
                    pokemonHeal = 729;
                    break;
                case 18:
                    pokemonHealth = 602;
                    pokemonAtk = 649;
                    pokemonSpAtk = 602;
                    pokemonHeal = 802;
                    break;
                case 19:
                    pokemonHealth = 662;
                    pokemonAtk = 714;
                    pokemonSpAtk = 827;
                    pokemonHeal = 882;
                    break;
                case 20:
                    pokemonHealth = 728;
                    pokemonAtk = 785;
                    pokemonSpAtk = 909;
                    pokemonHeal = 970;
                    break;
                case 21:
                    pokemonHealth = 1000;
                    pokemonAtk = 850;
                    pokemonSpAtk = 1000;
                    pokemonHeal = 1000;
                    break;
            }

            battlePokemonHealthBar = new Rectangle(200, 300, pokemonHealth / 5, 20);
        }


        //lv up pokemon lV
        private void evolveButton_Click(object sender, EventArgs e)
        {
            if (gameState == "Evolve Screen")
            {
                if (playerMoney >= moneyRequired[i] && pokemonLv < 21)
                {
                    playLevelUpSound();
                    pokemonLv++;
                    pokemonLvChecking();
                    playerMoney -= moneyRequired[i];
                    battlePokemonHealthBar = new Rectangle(200, 300, pokemonHealth / 5, 20);
                    i++;
                }
                else if (pokemonLv == 21)
                {
                    evolveButton.Enabled = false;
                }
                else
                {
                    evolveButton.Enabled = false;
                }
            }
        }

        //get the player to the store when button is clicked
        private void exitEvolve_Click(object sender, EventArgs e)
        {
            gameState = "House Interior";
            player1.Y = vendingMachine.Y + 120;
            evolveButton.Visible = false;
            exitEvolve.Visible = false;
            this.Focus();
        }

        //move the player to the battle screen when they intersect with the arena
        private void arenaEnterCollision()
        {
            if (gameState == "Arena Screen" && player1.IntersectsWith(enterArena))
            {
                gameState = "Battle Screen";
                InitializeBattleScreen();
            }
        }

        //Initialize for fighitng screen
        private void InitializeBattleScreen()
        {
            playBattleSound();
            pokemonLvChecking();
            gameState = "Battle Screen";
            battlePokemon.Image = playerPokemon;
            pokemonHealthLabel.Text = $"{pokemonHealth}";
            bossHealthLabel.Text = $"{bossHealth}";

            this.BackgroundImage = Properties.Resources.forest;

            resultBattleLabel.Visible = true;
            attackButton.Visible = true;
            healButton.Visible = true;
            spAttackButton.Visible = true;

            battlePokemon.Visible = true;
            battleBoss.Visible = true;

            attackButton.Enabled = true;
            healButton.Enabled = true;
            spAttackButton.Enabled = true;
            pokemonHealthLabel.Visible = true;
            bossHealthLabel.Visible = true;
            switch (bossCounter)
            {
                case 1:
                    battleBoss.Image = boss1;
                    bossHealth = 1000;
                    bossAtk = 400;
                    bossSpAtk = 500;
                    bossHeal = 400;
                    break;
                case 2:
                    battleBoss.Image = boss2;
                    battleBossHealthBar = new Rectangle(bossCurrentHealthX, 100, (bossHealth / 5), 20);
                    bossHealth = 1500;
                    bossAtk = 600;
                    bossSpAtk = 1000;
                    bossHeal = 500;
                    break;
                case 3:
                    battleBoss.Image = boss3;
                    battleBossHealthBar = new Rectangle(bossCurrentHealthX, 100, (bossHealth / 5), 20);
                    bossHealth = 2000;
                    bossAtk = 800;
                    bossSpAtk = 1200;
                    bossHeal = 600;
                    break;
            }
        }

        //player choice
        private void attackButton_Click(object sender, EventArgs e)
        {
            playerChoice = "Normal Attack";
            playNormalAtkSound();
            playerTurn();
        }
        private void healButton_Click(object sender, EventArgs e)
        {
            playerChoice = "Heal";
            playHealSound();
            playerTurn();
        }

        private void spAttackButton_Click(object sender, EventArgs e)
        {
            if (!playerSpAttackOnCooldown)
            {
                playerChoice = "Special Attack";
                playSpAtkSound();
                playerSpAttackOnCooldown = true;
                playerTurnCount = 0;  // Reset cooldown counter for player
                playerTurn();
            }
            else
            {
                // MessageBox.Show("Special Attack is on cooldown!");
            }
        }
        private void playerTurn()
        {
            playerFightingResult();
            Refresh();
            checkWinCondition();
            cpuTurn();
        }
        private void cpuTurn()
        {
            computerChoice();
            computerFightingResult();
            Refresh();
            checkWinCondition();

            // Update cooldown counters
            playerTurnCount++;
            cpuTurnCount++;

            // Reset cooldown if enough turns have passed
            if (playerTurnCount >= 3)
            {
                playerSpAttackOnCooldown = false;
                cooldownNotice.Visible = false;

            }

            if (cpuTurnCount >= 3)
            {
                cpuSpAttackOnCooldown = false;
            }
        }

        public void computerChoice()
        {
            pTurn.Visible = true;
            battleBossHealthBar = new Rectangle(bossCurrentHealthX, 100, (bossHealth / 5), 20);

            int randValue = randGen.Next(1, 100);
            if (cpuSpAttackOnCooldown && randValue < 60)
            {
                cpuChoice = "Normal Attack";
            }
            else if (cpuSpAttackOnCooldown && randValue < 90)
            {
                cpuChoice = "Heal";

            }
            else if (!cpuSpAttackOnCooldown && randValue < 30)
            {
                cpuChoice = "Heal";

            }
            else if (!cpuSpAttackOnCooldown && randValue < 90)
            {
                cpuChoice = "Special Attack";
                cpuSpAttackOnCooldown = true;
                cpuTurnCount = 0;
            }
            else
            {
                cpuChoice = "Normal Attack";
            }
            Thread.Sleep(500);
        }

        //player fighting results based on what they chose
        public void playerFightingResult()
        {
            pTurn.Visible = false;
            battleBossHealthBar = new Rectangle(bossCurrentHealthX, 100, (bossHealth / 5), 20);

            if (playerChoice == "Heal")
            {
                pokemonHealth += pokemonHeal;

            }
            else if (playerChoice == "Normal Attack")
            {
                bossHealth -= pokemonAtk;
                battleBossHealthBar = new Rectangle(bossCurrentHealthX, 100, (bossHealth / 5), 20);
            }
            else if (playerChoice == "Special Attack")
            {
                bossHealth -= pokemonSpAtk;
                battleBossHealthBar = new Rectangle(bossCurrentHealthX, 100, (bossHealth / 5), 20);
                cooldownNotice.Visible = true;
                playerTurnCount = 0;

            }
            pokemonHealthLabel.Text = $"{pokemonHealth}";
            bossHealthLabel.Text = $"{bossHealth}";
            battlePokemonHealthBar = new Rectangle(200, 300, pokemonHealth / 5, 20);
            Thread.Sleep(1000);
        }

        //cpu fighting results based on what they chose
        public void computerFightingResult()
        {

            if (cpuChoice == "Heal")
            {
                bossHealth += bossHeal;
                battleBossHealthBar = new Rectangle(bossCurrentHealthX, 100, (bossHealth / 5), 20);
            }
            else if (cpuChoice == "Normal Attack")
            {
                pokemonHealth -= bossAtk;
            }
            else if (cpuChoice == "Special Attack")
            {
                pokemonHealth -= bossSpAtk;
            }
            pokemonHealthLabel.Text = $"{pokemonHealth}";
            bossHealthLabel.Text = $"{bossHealth}";
            battlePokemonHealthBar = new Rectangle(200, 300, pokemonHealth / 5, 20);
            Thread.Sleep(1000);
        }

        //the end screen
        public void InitializeEndScreen()
        {
            bossHealthLabel.Visible = false;
            pokemonHealthLabel.Visible = false;

            attackButton.Visible = false;
            attackButton.Enabled = false;
            healButton.Visible = false;
            healButton.Enabled = false;
            spAttackButton.Visible = false;
            spAttackButton.Enabled = false;

            battleBoss.Visible = false;
            battlePokemon.Location = new Point(this.Width / 2, this.Height / 2);
            pTurn.Visible = false;
            cooldownNotice.Visible = false;

            gameState = "End Screen";
            Thread.Sleep(2000);
            playAgainLabel.Visible = true;
        }

        //w
        private void checkWinCondition()
        {
            if (gameState == "Battle Screen" && pokemonHealth <= 0)
            {
                pokemonLvChecking();
                InitializeBattleScreen();
                pokemonHealth = 0;
                pokemonHealthLabel.Text = "0";
                resultBattleLabel.Text = "You Loss";
                Thread.Sleep(500);
                pokemonLvChecking();
                resultBattleLabel.Text = " ";
                gameLoop.Enabled = false;
                InitializeMainScreen();
            }

            if (gameState == "Battle Screen" && bossHealth <= 0 && bossCounter == 1)
            {
                bossHealth = 0;
                bossHealthLabel.Text = "0";
                resultBattleLabel.Text = "Next Boss";
                battleBoss.Image = boss2;
                resultBattleLabel.Refresh();
                Thread.Sleep(500);
                resultBattleLabel.Text = " ";
                bossCounter++;
                InitializeBattleScreen();
            }

            if (gameState == "Battle Screen" && bossHealth <= 0 && bossCounter == 2)
            {
                bossHealth = 0;
                bossHealthLabel.Text = "0";
                battleBoss.Image = boss3;
                resultBattleLabel.Text = "Next Boss";
                Thread.Sleep(500);
                resultBattleLabel.Text = " ";
                bossCounter++;
                InitializeBattleScreen();
            }

            if (gameState == "Battle Screen" && bossHealth <= 0 && bossCounter == 3)
            {
                bossHealth = 0;
                bossHealthLabel.Text = "0";
                resultBattleLabel.Text = "You Win";
                Thread.Sleep(500);
                resultBattleLabel.Text = " ";
                gameLoop.Enabled = false;
                InitializeEndScreen();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "Start Screen")
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImage(forrestImg, grass);
                titleLabel.Image = Properties.Resources.Pokemon_Sign;
            }
            else if (gameState == "Choosing Screen")
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImage(forrestImg, grass);
            }
            else if (gameState == "Main Screen")
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImage(grassImg, grass);

                e.Graphics.DrawImage(GetCurrentDirectionImages()[player1FrameIndex], player1);

                for (int i = 0; i < treesList.Count; i++)
                {
                    e.Graphics.DrawImage(treeImgList[i], treesList[i]);
                }

                e.Graphics.DrawImage(chestImg, actualChest);

                e.Graphics.DrawImage(playerHouse, house);
                e.Graphics.DrawString($"{playerName}  LV:{pokemonLv}\nCoins:{playerMoney}", drawFont, blackBrush, 20, 30);
            }

            else if (gameState == "Arena Screen")
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImage(grassImg, grass);
                e.Graphics.DrawImage(GetCurrentDirectionImages()[player1FrameIndex], player1);
                e.Graphics.DrawImage(plantArena, grassArena);
                e.Graphics.DrawString($"LV:{pokemonLv}\nCoins:{playerMoney}", drawFont, blackBrush, 20, 30);
            }

            else if (gameState == "House Interior")
            {
                e.Graphics.Clear(Color.Black);
                e.Graphics.DrawImage(HouseInteriorImg, houseInterior);
                e.Graphics.DrawImage(GetCurrentDirectionImages()[player1FrameIndex], player1);
                e.Graphics.DrawString($"Coins:{playerMoney}", drawFont, blackBrush, 20, 30);
                e.Graphics.DrawImage(vendingMachineImg, vendingMachine);
            }
            else if (gameState == "Evolve Screen")
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImage(evolveScreenBg, 0, 0, 800, 500);
                e.Graphics.DrawImage(UpgradeStatsBg, statsBg);
                e.Graphics.DrawString($"{pokemonHealth}", drawFont, blackBrush, 670, 106);
                e.Graphics.DrawString($"{pokemonAtk}", drawFont, blackBrush, 670, 145);
                e.Graphics.DrawString($"{pokemonHeal}", drawFont, blackBrush, 670, 180);
                e.Graphics.DrawString($"{pokemonSpAtk}", drawFont, blackBrush, 670, 218);
                e.Graphics.DrawString($"${playerMoney}", drawFont, blackBrush, 670, 50);
                e.Graphics.DrawString($"{actualPokemon}", drawFont, blackBrush, 120, 50);


                if (pokemonLv < 21)
                {
                    e.Graphics.DrawString($"${moneyRequired[i]}", drawFont, blackBrush, 550, 283);
                    e.Graphics.DrawString($"Lvl:{pokemonLv}", drawFont, blackBrush, 505, 50);

                }
                e.Graphics.DrawString("Cost Per Evolution: ", drawFont, blackBrush, 100, 283);
                e.Graphics.DrawImage(playerPokemon, pokemonPicEvolve);

                if (i < moneyRequired.Length)
                {
                    if (playerMoney < moneyRequired[i] && pokemonLv < 21)
                    {
                        e.Graphics.DrawString($"${moneyRequired[i]}", drawFont, redBrush, 550, 283);
                    }
                }

                else
                {
                    evolveButton.Enabled = true;
                }
                if (pokemonLv == 21)
                {
                    e.Graphics.DrawString("Pokemon Maxed", drawFont, blackBrush, 550, 283);
                    e.Graphics.DrawString($"Lvl: Max", drawFont, blackBrush, 505, 50);
                }
            }
            else if (gameState == "Battle Screen")
            {
                e.Graphics.FillRectangle(redBrush, battleBossHealthBar);
                e.Graphics.FillRectangle(redBrush, battlePokemonHealthBar);
            }
            else if (gameState == "End Screen")
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImage(winImg, grass);
                titleLabel.Image = Properties.Resources.Pokemon_Sign;
            }
        }
        private void gameLoop_Tick(object sender, EventArgs e)
        {
            PlayerMovement();
            doorCollision();
            obstacleCollision();
            houseCollision();
            arenaCollision();
            InteriorCollision();
            chestCollision();
            arenaEnterCollision();
            checkWinCondition();
            Refresh();
        }
    }
}