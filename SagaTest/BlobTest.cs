using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schemas;
using System.Drawing;

namespace SagaTest
{

	/// <summary>
	/// Тестирование класса Blob
	/// </summary>
	[TestClass]
	public class BlobTest
	{
		/// <summary>
		/// Запись в Blob типа Int
		/// </summary>
		[TestMethod]
		public void Test_MakeBlob_Int()
		{
			const int testval = 10;
			var testtype = enAttrTypes.mnint;
			var attr1 = new AttrFactData(testtype, testval);
			var list = new List<AttrFactData>();
			list.Add(attr1);
			Blob blobpar = new Blob(list);

			var bdata = blobpar.Data;

			var tplist = new List<enAttrTypes>();
			tplist.Add(testtype);
			Blob blobchld = new Blob(tplist, bdata);

			var val = (int)blobchld.ValueList[0].Value;
			Assert.AreEqual(testval, val);
		}

		/// <summary>
		/// Запись в Blob типа FLoat
		/// </summary>
		[TestMethod]
		public void Test_MakeBlob_Float()
		{
			const float testval = 10.8F;
			var testtype = enAttrTypes.mnreal;
			var attr1 = new AttrFactData(testtype, testval);
			var list = new List<AttrFactData>();
			list.Add(attr1);
			Blob blobpar = new Blob(list);

			var bdata = blobpar.Data;

			var tplist = new List<enAttrTypes>();
			tplist.Add(testtype);
			Blob blobchld = new Blob(tplist, bdata);

			var val = (float)blobchld.ValueList[0].Value;
			Assert.AreEqual(testval, val);
		}

		/// <summary>
		/// Запись в Blob типа Bool
		/// </summary>
		[TestMethod]
		public void Test_MakeBlob_Bool()
		{
			const bool testval = true;
			var testtype = enAttrTypes.mnbool;
			var attr1 = new AttrFactData(testtype, testval);
			var list = new List<AttrFactData>();
			list.Add(attr1);
			Blob blobpar = new Blob(list);

			var bdata = blobpar.Data;

			var tplist = new List<enAttrTypes>();
			tplist.Add(testtype);
			Blob blobchld = new Blob(tplist, bdata);

			var val = (bool)blobchld.ValueList[0].Value;
			Assert.AreEqual(testval, val);
		}

		/// <summary>
		/// Запись в Blob типа string
		/// </summary>
		[TestMethod]
		public void Test_MakeBlob_String()
		{
			//const string testval = "value1";
			const string testval = "хорошо";
			var testtype = enAttrTypes.mntxt;
			var attr1 = new AttrFactData(testtype, testval);
			var list = new List<AttrFactData>();
			list.Add(attr1);
			Blob blobpar = new Blob(list);

			var bdata = blobpar.Data;

			var tplist = new List<enAttrTypes>();
			tplist.Add(testtype);
			Blob blobchld = new Blob(tplist, bdata);

			var val = (string)blobchld.ValueList[0].Value;
			Console.WriteLine(val);
			Assert.AreEqual(testval, val);
		}

		/// <summary>
		/// Запись в Blob типа LinksArray
		/// </summary>
		[TestMethod]
		public void Test_MakeBlob_Array()
		{
			const string testval = "10,11";
			var testtype = enAttrTypes.mnarr;
			var lst = new List<int>();
			lst.Add(10);
			lst.Add(11);

			var attr1 = new AttrFactData(testtype, lst);
			var list = new List<AttrFactData>();
			list.Add(attr1);
			Blob blobpar = new Blob(list);

			var bdata = blobpar.Data;

			var tplist = new List<enAttrTypes>();
			tplist.Add(testtype);
			Blob blobchld = new Blob(tplist, bdata);

			var val = (List<int>)blobchld.ValueList[0].Value;
			var strval = string.Join(",", val);
			Console.WriteLine(strval);
			Assert.AreEqual(testval, strval);
		}

		/// <summary>
		/// Запись в Blob типа Picture
		/// </summary>
		[TestMethod]
		public void Test_MakeBlob_Picture()
		{
			var testtype = enAttrTypes.mnblob;
			var fname = "Breakers.jpg";

			var img = Image.FromFile(@"c:/temp/" + fname);
			var attr1 = new AttrFactData(testtype, img);
			var list = new List<AttrFactData>();
			list.Add(attr1);
			Blob blobpar = new Blob(list);

			var bdata = blobpar.Data;

			var tplist = new List<enAttrTypes>();
			tplist.Add(testtype);
			Blob blobchld = new Blob(tplist, bdata);

			var val = (Image)blobchld.ValueList[0].Value;
			val.Save(@"c:/temp/new_" + fname);

			Assert.AreEqual(img, val);
		}

		/// <summary>
		/// Запись в Blob нескольких типов
		/// </summary>
		[TestMethod]
		public void Test_MakeBlob_SomeAttrs()
		{
			var list = new List<AttrFactData>();
			list.Add(new AttrFactData(enAttrTypes.mnbool, true));
			list.Add(new AttrFactData(enAttrTypes.mnint, 11));
			Blob blobpar = new Blob(list);

			var bdata = blobpar.Data;

			var tplist = new List<enAttrTypes>();
			tplist.Add(enAttrTypes.mnbool);
			tplist.Add(enAttrTypes.mnint);
			Blob blobchld = new Blob(tplist, bdata);

			var eq = true;
			eq = (bool)blobchld.ValueList[0].Value == true;
			eq = (int)blobchld.ValueList[1].Value == 11;

			Assert.AreEqual(eq, true);
		}

	}
}
