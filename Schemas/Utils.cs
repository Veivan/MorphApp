using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using FlatBuffers;

namespace TMorph.Common
{
	public static class Utils
    {
		/// <summary>
		/// Преобразование картинки в массив байт
		/// </summary>
		public static byte[] ImageToByte(Image img)
		{
			using (var stream = new MemoryStream()) 
			{
				img.Save(stream, img.RawFormat);
				//img.Save(stream, ImageFormat.Png);
				return stream.ToArray();
			}
		}

		/// <summary>
		/// Method that uses the ImageConverter object in .Net Framework to convert a byte array, 
		/// presumably containing a JPEG or PNG file image, into a Bitmap object, which can also be 
		/// used as an Image object.
		/// </summary>
		/// <param name="byteArray">byte array containing JPEG or PNG file image or similar</param>
		/// <returns>Bitmap object if it works, else exception is thrown</returns>
		public static Bitmap GetImageFromByteArray(byte[] byteArray)
		{
			ImageConverter _imageConverter = new ImageConverter();
			Bitmap bm = (Bitmap)_imageConverter.ConvertFrom(byteArray);

			if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
							   bm.VerticalResolution != (int)bm.VerticalResolution))
			{
				// Correct a strange glitch that has been observed in the test program when converting 
				//  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts" 
				//  slightly away from the nominal integer value
				bm.SetResolution((int)(bm.HorizontalResolution + 0.5f),
								 (int)(bm.VerticalResolution + 0.5f));
			}

			return bm;
		}
		public static byte[] BytesFromImage(Image image)
		{
			//Parse the image to a bitmap
			Bitmap bmp = new Bitmap(image);

			// Set the area we're interested in and retrieve the bitmap data
			Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

			// Create a byte array from the bitmap data
			int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
			byte[] rgbValues = new byte[bytes];
			IntPtr ptr = bmpData.Scan0;
			Marshal.Copy(ptr, rgbValues, 0, bytes);

			bmp.UnlockBits(bmpData);

			//return the byte array
			return rgbValues;
		}

		/*// <summary>
		 /// Функция получает "правильный" массив foo из builder.DataBuffer.Data.
		 /// Воссоздание ByteBuffer из ороигинального builder.DataBuffer.Data Root объект создаётся неправильно,
		 /// а из foo - правильно.
		 /// </summary>
		 /// <param name="buf"></param> оригинальный ByteBuffer
		 /// <returns>byte[] скопированный из buf.Data</returns>
		 public static byte[] FormatBuff(ByteBuffer buf)
		 {
			 byte[] foo = new byte[buf.Length - buf.Position];
			 System.Buffer.BlockCopy(buf.Data, buf.Position, foo, 0, buf.Length - buf.Position);
			 return foo;
		 } */


		/// <summary>
		/// Returns the string the user entered; empty string if they hit Cancel:
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="prompt"></param>
		/// <param name="defaultText"></param>
		/// <returns></returns>
		public static String InputBox(String caption, String prompt, String defaultText)
		{
			String localInputText = defaultText;
			if (InputQuery(caption, prompt, ref localInputText))
			{
				return localInputText;
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Returns the String as a ref parameter, returning true if they hit OK, or false if they hit Cancel
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="prompt"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Boolean InputQuery(String caption, String prompt, ref String value)
		{
			Form form;
			form = new Form();
			form.AutoScaleMode = AutoScaleMode.Font;
			form.Font = SystemFonts.IconTitleFont;

			SizeF dialogUnits;
			dialogUnits = form.AutoScaleDimensions;

			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.Text = caption;

			form.ClientSize = new Size(
						MulDiv(180, dialogUnits.Width, 4),
						MulDiv(63, dialogUnits.Height, 8));

			form.StartPosition = FormStartPosition.CenterScreen;

			System.Windows.Forms.Label lblPrompt;
			lblPrompt = new System.Windows.Forms.Label();
			lblPrompt.Parent = form;
			lblPrompt.AutoSize = true;
			lblPrompt.Left = MulDiv(8, dialogUnits.Width, 4);
			lblPrompt.Top = MulDiv(8, dialogUnits.Height, 8);
			lblPrompt.Text = prompt;

			System.Windows.Forms.TextBox edInput;
			edInput = new System.Windows.Forms.TextBox();
			edInput.Parent = form;
			edInput.Left = lblPrompt.Left;
			edInput.Top = MulDiv(19, dialogUnits.Height, 8);
			edInput.Width = MulDiv(164, dialogUnits.Width, 4);
			edInput.Text = value;
			edInput.SelectAll();


			int buttonTop = MulDiv(41, dialogUnits.Height, 8);
			//Command buttons should be 50x14 dlus
			Size buttonSize = ScaleSize(new Size(100, 50), dialogUnits.Width / 4, dialogUnits.Height / 8);

			System.Windows.Forms.Button bbOk = new System.Windows.Forms.Button();
			bbOk.Parent = form;
			bbOk.Text = "OK";
			bbOk.DialogResult = DialogResult.OK;
			form.AcceptButton = bbOk;
			bbOk.Location = new Point(MulDiv(38, dialogUnits.Width, 4), buttonTop);
			bbOk.Size = buttonSize;

			System.Windows.Forms.Button bbCancel = new System.Windows.Forms.Button();
			bbCancel.Parent = form;
			bbCancel.Text = "Cancel";
			bbCancel.DialogResult = DialogResult.Cancel;
			form.CancelButton = bbCancel;
			bbCancel.Location = new Point(MulDiv(92, dialogUnits.Width, 4), buttonTop);
			bbCancel.Size = buttonSize;

			if (form.ShowDialog() == DialogResult.OK)
			{
				value = edInput.Text;
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Multiplies two 32-bit values and then divides the 64-bit result by a 
		/// third 32-bit value. The final result is rounded to the nearest integer.
		/// </summary>
		private static int MulDiv(int nNumber, float nNumerator, int nDenominator)
		{
			return (int)Math.Round((float)nNumber * nNumerator / nDenominator);
		}
		private static Size ScaleSize(Size sz, float width, float height)
		{
			var nW = (int)Math.Round((float)sz.Width / width);
			var nH = (int)Math.Round((float)sz.Height / height);
			return new Size(nW, nH);
		}
	}
}
