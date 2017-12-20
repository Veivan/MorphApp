// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace TMorph.Schema
{

using global::System;
using global::FlatBuffers;

public struct Lexema : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Lexema GetRootAsLexema(ByteBuffer _bb) { return GetRootAsLexema(_bb, new Lexema()); }
  public static Lexema GetRootAsLexema(ByteBuffer _bb, Lexema obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Lexema __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public short Order { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0; } }
  public string EntryName { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetEntryNameBytes() { return __p.__vector_as_arraysegment(6); }
  public int IdEntry { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public short IdPartofspeech { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0; } }
  public Grammema? Grammems(int j) { int o = __p.__offset(12); return o != 0 ? (Grammema?)(new Grammema()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int GrammemsLength { get { int o = __p.__offset(12); return o != 0 ? __p.__vector_len(o) : 0; } }
  public long LexemaID { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }

  public static Offset<Lexema> CreateLexema(FlatBufferBuilder builder,
      short order = 0,
      StringOffset EntryNameOffset = default(StringOffset),
      int id_entry = 0,
      short id_partofspeech = 0,
      VectorOffset grammemsOffset = default(VectorOffset),
      long LexemaID = 0) {
    builder.StartObject(6);
    Lexema.AddLexemaID(builder, LexemaID);
    Lexema.AddGrammems(builder, grammemsOffset);
    Lexema.AddIdEntry(builder, id_entry);
    Lexema.AddEntryName(builder, EntryNameOffset);
    Lexema.AddIdPartofspeech(builder, id_partofspeech);
    Lexema.AddOrder(builder, order);
    return Lexema.EndLexema(builder);
  }

  public static void StartLexema(FlatBufferBuilder builder) { builder.StartObject(6); }
  public static void AddOrder(FlatBufferBuilder builder, short order) { builder.AddShort(0, order, 0); }
  public static void AddEntryName(FlatBufferBuilder builder, StringOffset EntryNameOffset) { builder.AddOffset(1, EntryNameOffset.Value, 0); }
  public static void AddIdEntry(FlatBufferBuilder builder, int idEntry) { builder.AddInt(2, idEntry, 0); }
  public static void AddIdPartofspeech(FlatBufferBuilder builder, short idPartofspeech) { builder.AddShort(3, idPartofspeech, 0); }
  public static void AddGrammems(FlatBufferBuilder builder, VectorOffset grammemsOffset) { builder.AddOffset(4, grammemsOffset.Value, 0); }
  public static VectorOffset CreateGrammemsVector(FlatBufferBuilder builder, Offset<Grammema>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartGrammemsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddLexemaID(FlatBufferBuilder builder, long LexemaID) { builder.AddLong(5, LexemaID, 0); }
  public static Offset<Lexema> EndLexema(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Lexema>(o);
  }
};


}
