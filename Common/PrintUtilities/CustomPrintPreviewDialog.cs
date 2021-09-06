using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Common.PrintUtilities
{
    public class CustomPrintPreviewDialog 
    {
       
       private double dblZoom;
       private int x, y;
       private MouseButtons clickRL;

       public PrintPreviewDialog PrintPreviewDlg 
       {
           get { return PrevDialog; }
       }
       private PrintPreviewDialog PrevDialog;

       public CustomPrintPreviewDialog()
       {
           PrevDialog = new PrintPreviewDialog();
           PrevDialog.Width = Screen.PrimaryScreen.Bounds.Width;
           PrevDialog.Height = Screen.PrimaryScreen.Bounds.Height;
           
           clickRL = new MouseButtons();

           PrevDialog.AutoSizeMode = AutoSizeMode.GrowAndShrink;
           PrevDialog.PrintPreviewControl.AutoZoom = true;
           dblZoom = 0.5;
           
           PrevDialog.PrintPreviewControl.Click += new EventHandler(PrintPreviewControl_Click);
           PrevDialog.PrintPreviewControl.MouseDown +=new MouseEventHandler(PrintPreviewControl_MouseDown);
            
       }

       public PrintPreviewDialog PreviewDialog(System.Drawing.Printing.PrintDocument document)
       {           
           PrevDialog.PrintPreviewControl.Document = document;
           return PrevDialog;
       }

       #region // this is the events method
       public void PrintPreviewControl_Click(object obj, EventArgs e)
       {
           
           

           //PrevDialog.PrintPreviewControl.AutoScrollOffset = new System.Drawing.Point(100, 0);
           if (clickRL == MouseButtons.Left)
           {
               dblZoom += 0.3;
//               PrevDialog.Width = Screen.PrimaryScreen.Bounds.Width;
//               PrevDialog.Height = Screen.PrimaryScreen.Bounds.Height;
               //           MessageBox.Show(PrevDialog.PrintPreviewControl.Zoom.ToString());
               PrevDialog.PrintPreviewControl.Zoom = dblZoom;
           }
           else if(clickRL == MouseButtons.Right)
           {
               dblZoom -= 0.3;
               if (dblZoom <= 0)
               {
                   dblZoom += 0.3;
               }
               else
               {
  //                 PrevDialog.Width = Screen.PrimaryScreen.Bounds.Width;
  //                 PrevDialog.Height = Screen.PrimaryScreen.Bounds.Height;
                   PrevDialog.PrintPreviewControl.Zoom = dblZoom;
               }
           }
           
       }

       private void PrintPreviewControl_MouseDown(object obj, MouseEventArgs e)
       {
           //this.PrintPreviewDlg.hori

           clickRL = e.Button;
           x = e.X;
           y = e.Y;
       }

     
       #endregion

        
   }
}
