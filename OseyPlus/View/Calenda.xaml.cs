using Microsoft.Maui.Controls;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OseyPlus.View
{
    public class ExpenseChartPage : ContentPage
    {
        private List<ExpenseData> expenses = new List<ExpenseData>
        {
            new ExpenseData { Month = "Jan", Amount = 100 },
            new ExpenseData { Month = "Feb", Amount = 150 },
            new ExpenseData { Month = "Mar", Amount = 200 },
            // ... Ajoutez vos données pour chaque mois
        };

        public ExpenseChartPage()
        {
            Title = "Courbe de Dépenses";

            var chartView = new SkiaSharp.Views.Maui.Controls.SKCanvasView();
            chartView.PaintSurface += OnCanvasViewPaintSurface;

            Content = new StackLayout
            {
                Padding = new Thickness(10),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { chartView }
            };
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            // Définir les marges et la taille du graphique
            float margin = 20;
            float width = e.Info.Width - 2 * margin;
            float height = e.Info.Height - 2 * margin;

            // Trouver le montant maximum pour normaliser les données
            float maxAmount = expenses.Max(expense => expense.Amount);

            // Calculer l'échelle pour les axes X et Y
            float scaleX = width / (expenses.Count - 1);
            float scaleY = height / maxAmount;

            // Dessiner les axes
            canvas.DrawLine(margin, margin, margin, height + margin, new SKPaint { Color = SKColors.Black });
            canvas.DrawLine(margin, height + margin, width + margin, height + margin, new SKPaint { Color = SKColors.Black });

            // Dessiner la courbe en utilisant les données
            SKPath path = new SKPath();
            path.MoveTo(margin, height - expenses.First().Amount * scaleY + margin);

            for (int i = 0; i < expenses.Count; i++)
            {
                float x = i * scaleX + margin;
                float y = height - expenses[i].Amount * scaleY + margin;

                path.LineTo(x, y);

                // Dessiner des cercles pour chaque point de données
                canvas.DrawCircle(x, y, 5, new SKPaint { Color = SKColors.Blue });
            }

            // Dessiner la courbe
            canvas.DrawPath(path, new SKPaint { Color = SKColors.Red, StrokeWidth = 3, IsAntialias = true });
        }

        private class ExpenseData
        {
            public string Month { get; set; }
            public float Amount { get; set; }
        }
    }
}
