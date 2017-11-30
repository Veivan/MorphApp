using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZeroMQ;
using FlatBuffers;
using TMorph.Schema;
using MyGame.Sample;
using TMorph.Common;

namespace HWClient
{
	class Program
	{
		static void Main(string[] args)
		{

			var builder = SetReq();
			var buf = builder.DataBuffer;
			var message = Message.GetRootAsMessage(buf);
			Console.WriteLine(" ServerType : {0}", message.ServerType.ToString());
			Console.WriteLine(" Comtype : {0}", message.Comtype.ToString());

			/*byte[] foo = new byte[buf.Length - buf.Position];
			System.Buffer.BlockCopy(buf.Data, buf.Position, foo, 0, buf.Length - buf.Position);*/
            var foo = Utils.FormatBuff(buf);

			var buf2 = new ByteBuffer(foo);
			//var buf2 = new ByteBuffer(builder.DataBuffer.Data);
			var message2 = Message.GetRootAsMessage(buf2);

			Console.WriteLine(" ServerType : {0}", message2.ServerType.ToString());
			Console.WriteLine(" Comtype : {0}", message2.Comtype.ToString());

/*
			var builder3 = MakeMonster();
			var buf3 = builder3.DataBuffer;
			var monster = Monster.GetRootAsMonster(buf3);
			Console.WriteLine(" monster.Name : {0}", monster.Name);

			byte[] foo = new byte[buf3.Length - buf3.Position];
			System.Buffer.BlockCopy(buf3.Data, buf3.Position, foo, 0, buf3.Length - buf3.Position);
			var buf4 = new ByteBuffer(foo);

			var monster4 = Monster.GetRootAsMonster(buf4);
			Console.WriteLine(" monster.Name : {0}", monster4.Name);
*/

			// Create
			// using (var context = new ZContext())
			using (var requester = new ZSocket(ZSocketType.REQ))
			{
				// Connect
				requester.Connect("tcp://127.0.0.1:5555");

				/*for (int n = 0; n < 10; ++n)
				{
					string requestText = "Hello";
					Console.Write("Sending {0}...", requestText);

					// Send
					requester.Send(new ZFrame(requestText));

					// Receive
					using (ZFrame reply = requester.ReceiveFrame())
					{
						Console.WriteLine(" Received: {0} {1}!", requestText, reply.ReadString());
					}
				}*/

				Console.Write("Sending message...");
				// Send

				//requester.Send(new ZFrame(builder.DataBuffer.Data));
				requester.Send(new ZFrame(foo));

				// Receive
				using (ZFrame reply = requester.ReceiveFrame())
				{
					//Console.WriteLine(" Received: {0} {1}!", requestText, reply.ReadString());
                    reply.Position = 0;
                    var bufrep = reply.Read();

                    PrintRep(bufrep);
                }

			}

			Console.ReadKey();
		}

		static FlatBufferBuilder SetReq()
		{
			var builder = new FlatBufferBuilder(1);
			var param1Name = builder.CreateString("phrase");
			var param1Val = builder.CreateString("мама мыла раму");
			var parms = new Offset<Param>[1];
			parms[0] = Param.CreateParam(builder, param1Name, param1Val);
			var paracol = Message.CreateParamsVector(builder, parms); 

			Message.StartMessage(builder);
			Message.AddMessType(builder, MessType.mRequest);
			Message.AddServerType(builder, ServType.svSUBD);
			Message.AddComtype(builder, ComType.Synt);
			Message.AddParams(builder, paracol);
			var req = Message.EndMessage(builder);
			builder.Finish(req.Value);

			return builder;
		}

        static void PrintRep(byte[] req)
        {
            var buf = new ByteBuffer(req);
            var message = Message.GetRootAsMessage(buf);
            Console.WriteLine(" ServerType : {0}", message.ServerType.ToString());
            Console.WriteLine(" Comtype : {0}", message.Comtype.ToString());

            for (int i = 0; i < message.ParamsLength; i++)
            {
                Param? par = message.Params(i);
                if (par == null) continue;
                Console.WriteLine(" Param : {0} = {1}", par.Value.Name, par.Value.Value);
            }
        }

		static FlatBufferBuilder MakeMonster()
		{
			var builder = new FlatBufferBuilder(1);

			// Create some weapons for our Monster ('Sword' and 'Axe').
			var weapon1Name = builder.CreateString("Sword");
			var weapon1Damage = 3;
			var weapon2Name = builder.CreateString("Axe");
			var weapon2Damage = 5;

			// Use the `CreateWeapon()` helper function to create the weapons, since we set every field.
			var weaps = new Offset<Weapon>[2];
			weaps[0] = Weapon.CreateWeapon(builder, weapon1Name, (short)weapon1Damage);
			weaps[1] = Weapon.CreateWeapon(builder, weapon2Name, (short)weapon2Damage);

			// Serialize the FlatBuffer data.
			var name = builder.CreateString("Orc");
			var treasure = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var inv = Monster.CreateInventoryVector(builder, treasure);
			var weapons = Monster.CreateWeaponsVector(builder, weaps);
			var pos = Vec3.CreateVec3(builder, 1.0f, 2.0f, 3.0f);

			Monster.StartMonster(builder);
			Monster.AddPos(builder, pos);
			Monster.AddHp(builder, (short)300);
			Monster.AddName(builder, name);
			Monster.AddInventory(builder, inv);
			Monster.AddColor(builder, Color.Red);
			Monster.AddWeapons(builder, weapons);
			Monster.AddEquippedType(builder, Equipment.Weapon);
			Monster.AddEquipped(builder, weaps[1].Value);
			var orc = Monster.EndMonster(builder);

			builder.Finish(orc.Value);
			return builder;
		}
	}
}
