using oop3.DisplayObjects;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    internal class Game
    { 

        private GameField gameField;
        public Game(GameField field) {
            gameField = field;
        }

        public bool HandleClick(int x, int y) {
            return gameField.Click(x, y);
        }

        public void DrawGame(Graphics g) {
            gameField.Draw(g);
        }

        // oh no
        public (int, int) GetClientOffset() {
            return (gameField.clientX1, gameField.clientY1);
        }

        public void MoveObjects(int time) { 
            gameField.MoveObjects(time);
        }
    }
}
