using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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

		/// <summary>
		/// Получение первого INT из хранящихся в byte[]
		/// </summary>
		/// <param name="byteArray">byte array containing JPEG or PNG file image or similar</param>
		/// <returns>Bitmap object if it works, else exception is thrown</returns>
		public static int GetFirstIntFromBytes(byte[] byteArray)
		{
			const int intlen = 4; // количество байт для хранения числа
			int offset = 0; // Текущая позиция чтения в _bytedata
			var arrbt = new byte[intlen];
			for (int i = 0; i < intlen; i++)
				arrbt[i] = byteArray[offset++];
			if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
			var result = BitConverter.ToInt32(arrbt, 0);
			return result;
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
	}
}
