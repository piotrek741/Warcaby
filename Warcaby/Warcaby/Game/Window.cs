using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Warcaby
{
    internal sealed class Window : ApplicationContext
    {
        internal Window(Engine engine)
        {
            this.window = new Form();
            this.engine = engine;

            Init();
        }

        private void Init()
        {
            window.ShowIcon = false;
            window.MaximizeBox = false;
            window.MinimizeBox = false;
            window.ClientSize = CalculateGameArea();
            window.StartPosition = FormStartPosition.CenterScreen;
            window.BackColor = Config.WAR_AREA_COLOR;
            
            window.Paint += (a, e) =>
            {
                Graphics graphics = e.Graphics;

                Size areaSize = new Size(Config.AREA_SIZE, Config.AREA_SIZE);
                Brush warAreaBrush = new SolidBrush(Config.WAR_AREA_COLOR);
                Brush emptyAreaBrush = new SolidBrush(Config.EMPTY_AREA_COLOR);
                Brush playerABrush = new SolidBrush(Config.PLAYER_A_COLOR);
                Brush playerBBrush = new SolidBrush(Config.PLAYER_B_COLOR);

                for (int row = 0; row < Config.AREA_COUNT; row++)
                {
                    for (int column = 0; column < Config.AREA_COUNT; column++)
                    {
                        Point currentPoint = CalculateAreaPoint(row, column);

                        Field field = engine.Board[row, column];

                        if (field.FieldType == FieldType.EmptyArea) {
                            graphics.FillRectangle(emptyAreaBrush, new Rectangle(currentPoint, areaSize));
                        } else if (field.FieldType == FieldType.WarArea) {
                            graphics.FillRectangle(warAreaBrush, new Rectangle(currentPoint, areaSize));
                        } else if (field.FieldType == FieldType.PlayerA) {
                            graphics.FillRectangle(warAreaBrush, new Rectangle(currentPoint, areaSize));
                            graphics.DrawImage(Checkers.A, new Rectangle(currentPoint.X + areaSize.Width / 4, currentPoint.Y + areaSize.Height / 4, areaSize.Width / 2, areaSize.Height / 2));
                        } else if (field.FieldType == FieldType.PlayerB) {
                            graphics.FillRectangle(warAreaBrush, new Rectangle(currentPoint, areaSize));
                            graphics.DrawImage(Checkers.B, new Rectangle(currentPoint.X + areaSize.Width / 4, currentPoint.Y + areaSize.Height / 4, areaSize.Width / 2, areaSize.Height / 2));
                        } else if (field.FieldType == FieldType.PlayerAQueen) {
                            graphics.FillRectangle(warAreaBrush, new Rectangle(currentPoint, areaSize));
                            // todo - pionek do podmiany na krolowa
                            graphics.DrawImage(Checkers.AQueen, new Rectangle(currentPoint.X + areaSize.Width / 4, currentPoint.Y + areaSize.Height / 4, areaSize.Width / 2, areaSize.Height / 2));
                        } else if (field.FieldType == FieldType.PlayerBQueen) {
                            graphics.FillRectangle(warAreaBrush, new Rectangle(currentPoint, areaSize));
                            // todo - pionek do podmiany na krolowa
                            graphics.DrawImage(Checkers.BQueen, new Rectangle(currentPoint.X + areaSize.Width / 4, currentPoint.Y + areaSize.Height / 4, areaSize.Width / 2, areaSize.Height / 2));
                        }
                    }
                }
            };
            window.MouseClick += (a, e) => {
                if (selectedPoint == null && engine.IsPossibleSource(CalculateRow(e.Y), CalculateColumn(e.X))) {
                    selectedPoint = e.Location;
                    window.Cursor = Cursors.Hand;
                } else if (selectedPoint != null) {
                    bool ok = engine.Move(CalculateRow(selectedPoint.Value.Y), CalculateColumn(selectedPoint.Value.X), CalculateRow(e.Y), CalculateColumn(e.X));

                    selectedPoint = null;
                    window.Cursor = Cursors.Default;
                    window.Refresh();

                    if (ok)
                    {
                        if (engine.End)
                        {
                            SystemSounds.Beep.Play();
                            MessageBox.Show("Koniec gry");
                            window.Close();
                        }
                        else
                        {
                            UpdateStatus();
                        }
                    }
                }
            };
            window.Shown += (a, b) => UpdateStatus();

            MainForm = window;
        }

        private void UpdateStatus()
        {
            window.Text = $"Punkty: {engine.Points[PlayerType.A]} : {engine.Points[PlayerType.B]}  |  Ruch dla gracza {engine.CurrentPlayer.ToString()}";
        }
        private Size CalculateGameArea()
        {
            int width = Config.AREA_COUNT * Config.AREA_SIZE;
            int height = Config.AREA_COUNT * Config.AREA_SIZE;

            return new Size(width, height);
        }
        private int CalculateRow(int pointX)
        {
            int row = 0;

            for (int i = 0; i < Config.AREA_COUNT; i++)
                if (pointX >= i * Config.AREA_SIZE)
                    row = i;

            return row;
        }
        private int CalculateColumn(int pointY)
        {
            int column = 0;

            for (int i = 0; i < Config.AREA_COUNT; i++)
                if (pointY >= i * Config.AREA_SIZE)
                    column = i;

            return column;
        }
        private Point CalculateAreaPoint(int row, int column)
        {
            int pointX = column * Config.AREA_SIZE;
            int pointY = row * Config.AREA_SIZE;

            return new Point(pointX, pointY);
        }

        private Point? selectedPoint;
        private readonly Form window;
        private readonly Engine engine;
    }
}