﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    /// <summary>
    /// Adds an extra <see cref="SnakeBit"/> to the end and increments the score
    /// </summary>
    public class FoodPickupHandler : IItemPickupHandler
    {
        private readonly Random _randomiser;

        public FoodPickupHandler(Random randomiser)
        {
            _randomiser = randomiser;
        }

        public char Item => BoardPiece.Food;

        public bool HandleItem(Game game, (int X, int Y) pos, out char item)
        {
            game.Score++;
            var (tX, tY) = game.Snake.Position;
            game.Snake = new SnakeBit(tX, tY, game.Snake);

            // occasionally add a new item to the board
            if (_randomiser.Next(0, 10) == 1)
            {
                var empty = game.Board.GetEmpty().ToList();

                if (empty.Count != 0)
                {
                    var (x, y) = empty[_randomiser.Next(0, empty.Count)];

                    game.Board.Update(x, y,
                        game.ItemsUsed.Where(i => i != BoardPiece.Food).ToList()[_randomiser.Next(0, game.ItemsUsed.Count() - 1)]);
                }
            }

            item = Item;
            // this item has no effect on other players
            return false;
        }
    }
}
