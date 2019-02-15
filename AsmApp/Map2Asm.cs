using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Schemas;
using AsmApp.Types;

namespace AsmApp
{
	class Map2Asm
	{
		public static SentenceAsm Convert(SentenceMap smap)
		{
			var asm = new SentenceAsm();

			for (int i = 0; i < smap.Capasity; i++)
			{
				var wmap = smap.GetWordByOrder(i);
				var wasm = new WordAsm(wmap.ID_Entry, wmap.ID_PartOfSpeech);
				wasm.xPart = wmap.xPart;
				wasm.EntryName = wmap.EntryName;
				wasm.Order = wmap.order;
				wasm.RealWord = wmap.RealWord;
				wasm.rcind = wmap.rcind;
				// Чтение граммем
				var Grammems = wmap.GetPairs();
				foreach(var g in Grammems)
				{
					var asmgramm = new GrammemAsm(g.Key, g.Value);
					wasm.AddGrammem(asmgramm);
				}
				asm.AddWord(i, wasm);
			}

			var mapNodes = smap.GetTreeList();

			foreach (var node in mapNodes)
			{
				var asmgsynt = new SyntNodeAsm(-1, node.linktype, node.Level, -1, node.index, node.parentind);
				asm.AddNode(asmgsynt);
			}

			return asm;
		}
	}
}
