// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace TMorph.Schema
{

using global::System;
using global::FlatBuffers;

public struct Grammema : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Grammema GetRootAsGrammema(ByteBuffer _bb) { return GetRootAsGrammema(_bb, new Grammema()); }
  public static Grammema GetRootAsGrammema(ByteBuffer _bb, Grammema obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Grammema __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Key { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Value { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<Grammema> CreateGrammema(FlatBufferBuilder builder,
      int key = 0,
      int value = 0) {
    builder.StartObject(2);
    Grammema.AddValue(builder, value);
    Grammema.AddKey(builder, key);
    return Grammema.EndGrammema(builder);
  }

  public static void StartGrammema(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddKey(FlatBufferBuilder builder, int key) { builder.AddInt(0, key, 0); }
  public static void AddValue(FlatBufferBuilder builder, int value) { builder.AddInt(1, value, 0); }
  public static Offset<Grammema> EndGrammema(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Grammema>(o);
  }
};


}
