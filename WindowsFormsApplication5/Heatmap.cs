using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Heatmap : Form
    {
        const int HEATMAP_COLS = 19;
        const int HEATMAP_ROWS = 8;

        string IMG_PATH
        {
            get
            {
                return LocalServerConn.root_folder_path + @"\FieldCropped.PNG";
            }
        }

        public struct Position
        {
            public char col;
            public int row;

            public string ToString()
            {
                return col + row.ToString();
            }

            public Position(char p_col, int p_row)
            {
                col = p_col;
                row = p_row;
            }
        }

        public Heatmap(List<KeyValuePair<Position, int>> p_positions)
        {
            InitializeComponent();

            List<KeyValuePair<Point, int>> points = new List<KeyValuePair<Point, int>>();

            foreach (var pos in p_positions)
            {
                points.Add
                (
                    new KeyValuePair<Point, int>
                    (
                        new Point(pos.Key.col.ToString().ToUpper()[0] - 'A', pos.Key.row - 1),
                        pos.Value
                    )
                );
            }

            Bitmap img = (Bitmap)Image.FromFile(IMG_PATH);

            DrawGrid(img);

            Size block_size = new Size(img.Width / HEATMAP_COLS, img.Height / HEATMAP_ROWS);

            List<Rectangle> rects = new List<Rectangle>();
            
            int max_val = 0;
            foreach (var point in points)
            {
                rects.Add(new Rectangle(new Point(point.Key.X * block_size.Width, point.Key.Y * block_size.Height), block_size));

                if (max_val < point.Value)
                {
                    max_val = point.Value;
                }
            }

            SolidBrush sb;
            float opacity = 0.75f * 255;
            using (Graphics g = Graphics.FromImage(img))
            {
                for (int i = 0; i < rects.Count; i++)
                {
                    sb = new SolidBrush(Color.FromArgb((int)(((float)points[i].Value / max_val) * opacity), 0, 255, 0));
                    g.FillRectangle(sb, rects[i]);
                }
            }

            pictureBox1.Image = img;
        }

        private void DrawGrid(Bitmap img)
        {
            using (Graphics g = Graphics.FromImage(img))
            {
                int x, y;
                Point p1, p2;
                Pen pen = Pens.Black;
                //List<Point> points = new List<Point>();
                for (int i = 0; i < HEATMAP_COLS - 1; i++)
                {
                    x = (i + 1) * (img.Width / HEATMAP_COLS);
                    p1 = new Point(x, 0);
                    p2 = new Point(x, img.Height - 1);
                    g.DrawLine(pen, p1, p2);
                }

                for (int i = 0; i < HEATMAP_ROWS; i++)
                {
                    y = (i + 1) * (img.Height / HEATMAP_ROWS);
                    p1 = new Point(0, y);
                    p2 = new Point(img.Width - 1, y);
                    g.DrawLine(pen, p1, p2);
                }

                //g.DrawLines(Pens.Black, points.ToArray());
            }

        }

        public static int GetHeatmapPropId()
        {
            var prop_styles = LocalServerConn.GetGSPropStyles();
            int index = prop_styles.IndexOf("Heatmap");

            return index;
        }
    }
}
