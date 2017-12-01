using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbMQserver
{
	class SQLiteConnector : IdbConnector
	{
		public long SaveLex(string word)
		{
			throw new NotImplementedException();
		}

		public long GetWord(string rword)
		{
			throw new NotImplementedException();
		}

		public long SavePhrase(long ph_id)
		{
			throw new NotImplementedException();
		}

		public long SavePhraseContent(long ph_id, long w_id)
		{
			throw new NotImplementedException();
		}
	}
}
