using System;
using System.Collections.Generic;
using Schemas;

namespace MorphApp
{
	class SagaStoreServer : IDataDealer
	{
		public override RetValue ReadContainers()
		{
			throw new NotImplementedException();
		}

		public override long SaveParagraph(long pg_id, List<SentenceMap> sentlist)
		{
			throw new NotImplementedException();
		}

		public override List<SentenceMap> ReadParagraphDB(long pg_id)
		{
			throw new NotImplementedException();
		}
	}
}
