using GameComponents.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameComponents
{
    public class GameController
    {
        public GameState GameState { get; private set; } = new GameState();
     
        public GameController() { }
        public GameController(GameState gameState) {  GameState = gameState; }

        /// <summary>
        /// Initializes the game with the given players. Each player gets a random position on the map.
        /// Checks if number of players is valid.
        /// </summary>
        /// <returns>True if game was started</returns>
        public bool StartGame()
        {
            Console.WriteLine("Game starting");
            if (GameState.MisterX == null)
            {
                Console.WriteLine("MisterX still missing");
                Console.WriteLine("Game not started");
                return false;
            }
            if (GameState.Detectives.Count < 1) //todo: How many detectives are needed?
            {
                Console.WriteLine("Not enough detectives");
                Console.WriteLine("Game not started");
                return false;
            }
            Initializer.InitPois(GameState);
            Initializer.InitPlayers(GameState, GameState.Detectives.Count + 1); // + MisterX

            GameState.ActivePlayer = GameState.MisterX;

            GameState.Move = 1;
            Console.WriteLine("Game started");
            return true;
        }
        /// <summary>
        /// Allows to reset the game. All players and pois are removed. Start game must be called again.
        /// </summary>
        public void ResetGame() 
        { 
            Console.WriteLine("Game reset");
            GameState = new GameState();
        }

        /// <summary>
        /// Adds players to the game. MisterX is added first, then detectives.
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>True if added</returns>
        public bool AddPlayer(Player player)
        {
            if (GameState.GameStarted)
            {
                Console.WriteLine("Player not added");
                return false;
            }
            else if (GameState.MisterX == null)
            {
                GameState.MisterX = player;
                GameState.MisterX.BlackTicket = 5;
                GameState.MisterX.DoubleTicket = 2;
                //free tickets for X!!!!!!!!!!
                GameState.MisterX.BusTicket = int.MaxValue;
                GameState.MisterX.BikeTicket = int.MaxValue;
                GameState.MisterX.ScooterTicket = int.MaxValue;
                Console.WriteLine("MisterX added");
                return true;
            }
            else if (GameState.Detectives.Count < 5)
            {
                GameState.Detectives.Add(player);
                Console.WriteLine("Detective added");
                return true;
            }
            else
            {
                Console.WriteLine("Player not added");
                return false;
            }
        }
        /// <summary>
        /// Moves the player after validating the move. Also increments the round counter.
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="poi">Point Of Interes</param>
        /// <param name="ticketType">Ticket that will be used</param>
        /// <returns>True if player was moved</returns>
        public bool MovePlayer(Player player, PointOfInterest poi, TicketTypeEnum ticketType)
        { 
            //exchange player with player from GameState.
            //This is necessary because the position attribute of the incoming player is null. (No valid position)
            //The GameState poi is needed to validate the move because the validator depends on it.
            //todo: force poi numbers to be unique
            var gamePlayers = GameState.AllPlayers.Where(p => p.UserName == player.UserName);
            var gamePois = GameState.PointsOfInterest.Where(p => p.Number == poi.Number);
            //check if player and poi are found in the game
            if (gamePlayers == null || gamePois == null || !(gamePlayers.Any() && gamePois.Any()))
            {
                Console.WriteLine("Player not moved");
                return false;
            }
            else if (GameState.GameStarted && GameState.ActivePlayer == gamePlayers.First()
                    && Validator.ValidateMove(
                    gamePlayers.First(), 
                    gamePois.First(), 
                    ticketType, 
                    GameState.Detectives))      
            {
                player.Position = poi;
                Console.WriteLine("Player moved");
                NextRound();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the active player to the next player. Also increases the move counter.
        /// </summary>
        private void NextRound()
        {
            GameState.ActivePlayer = GameState.AllPlayers[GameState.Move++ % GameState.AllPlayers.Count];
        }

    }
}
