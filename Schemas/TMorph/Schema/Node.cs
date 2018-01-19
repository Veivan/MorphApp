// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace TMorph.Schema
{

using global::System;
using global::FlatBuffers;

public struct Node : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Node GetRootAsNode(ByteBuffer _bb) { return GetRootAsNode(_bb, new Node()); }
  public static Node GetRootAsNode(ByteBuffer _bb, Node obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Node __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int ID { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Level { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Index { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Linktype { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Parentind { get { int o = __p.__offset(12); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public long NodeID { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }

  public static Offset<Node> CreateNode(FlatBufferBuilder builder,
      int ID = 0,
      int Level = 0,
      int index = 0,
      int linktype = 0,
      int parentind = 0,
      long NodeID = 0) {
    builder.StartObject(6);
    Node.AddNodeID(builder, NodeID);
    Node.AddParentind(builder, parentind);
    Node.AddLinktype(builder, linktype);
    Node.AddIndex(builder, index);
    Node.AddLevel(builder, Level);
    Node.AddID(builder, ID);
    return Node.EndNode(builder);
  }

  public static void StartNode(FlatBufferBuilder builder) { builder.StartObject(6); }
  public static void AddID(FlatBufferBuilder builder, int ID) { builder.AddInt(0, ID, 0); }
  public static void AddLevel(FlatBufferBuilder builder, int Level) { builder.AddInt(1, Level, 0); }
  public static void AddIndex(FlatBufferBuilder builder, int index) { builder.AddInt(2, index, 0); }
  public static void AddLinktype(FlatBufferBuilder builder, int linktype) { builder.AddInt(3, linktype, 0); }
  public static void AddParentind(FlatBufferBuilder builder, int parentind) { builder.AddInt(4, parentind, 0); }
  public static void AddNodeID(FlatBufferBuilder builder, long NodeID) { builder.AddLong(5, NodeID, 0); }
  public static Offset<Node> EndNode(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Node>(o);
  }
};


}
