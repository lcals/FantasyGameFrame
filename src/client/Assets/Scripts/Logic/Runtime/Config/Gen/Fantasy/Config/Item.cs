// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Fantasy.Config
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

/// Item 
public struct Item : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static Item GetRootAsItem(ByteBuffer _bb) { return GetRootAsItem(_bb, new Item()); }
  public static Item GetRootAsItem(ByteBuffer _bb, Item obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Item __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  /// id
  public int Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  /// name
  public string Name { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetNameBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetNameArray() { return __p.__vector_as_array<byte>(6); }
  /// item type
  public Fantasy.Config.ItemType ItemType { get { int o = __p.__offset(8); return o != 0 ? (Fantasy.Config.ItemType)__p.bb.GetInt(o + __p.bb_pos) : Fantasy.Config.ItemType.Unknown; } }
  ///describe
  public string Describe { get { int o = __p.__offset(10); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetDescribeBytes() { return __p.__vector_as_span<byte>(10, 1); }
#else
  public ArraySegment<byte>? GetDescribeBytes() { return __p.__vector_as_arraysegment(10); }
#endif
  public byte[] GetDescribeArray() { return __p.__vector_as_array<byte>(10); }

  public static Offset<Fantasy.Config.Item> CreateItem(FlatBufferBuilder builder,
      int id = 0,
      StringOffset nameOffset = default(StringOffset),
      Fantasy.Config.ItemType item_type = Fantasy.Config.ItemType.Unknown,
      StringOffset describeOffset = default(StringOffset)) {
    builder.StartTable(4);
    Item.AddDescribe(builder, describeOffset);
    Item.AddItemType(builder, item_type);
    Item.AddName(builder, nameOffset);
    Item.AddId(builder, id);
    return Item.EndItem(builder);
  }

  public static void StartItem(FlatBufferBuilder builder) { builder.StartTable(4); }
  public static void AddId(FlatBufferBuilder builder, int id) { builder.AddInt(0, id, 0); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(1, nameOffset.Value, 0); }
  public static void AddItemType(FlatBufferBuilder builder, Fantasy.Config.ItemType itemType) { builder.AddInt(2, (int)itemType, 0); }
  public static void AddDescribe(FlatBufferBuilder builder, StringOffset describeOffset) { builder.AddOffset(3, describeOffset.Value, 0); }
  public static Offset<Fantasy.Config.Item> EndItem(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<Fantasy.Config.Item>(o);
  }
  public ItemT UnPack() {
    var _o = new ItemT();
    this.UnPackTo(_o);
    return _o;
  }
  public void UnPackTo(ItemT _o) {
    _o.Id = this.Id;
    _o.Name = this.Name;
    _o.ItemType = this.ItemType;
    _o.Describe = this.Describe;
  }
  public static Offset<Fantasy.Config.Item> Pack(FlatBufferBuilder builder, ItemT _o) {
    if (_o == null) return default(Offset<Fantasy.Config.Item>);
    var _name = _o.Name == null ? default(StringOffset) : builder.CreateString(_o.Name);
    var _describe = _o.Describe == null ? default(StringOffset) : builder.CreateString(_o.Describe);
    return CreateItem(
      builder,
      _o.Id,
      _name,
      _o.ItemType,
      _describe);
  }
}

public class ItemT
{
  [Newtonsoft.Json.JsonProperty("id")]
  public int Id { get; set; }
  [Newtonsoft.Json.JsonProperty("name")]
  public string Name { get; set; }
  [Newtonsoft.Json.JsonProperty("item_type")]
  public Fantasy.Config.ItemType ItemType { get; set; }
  [Newtonsoft.Json.JsonProperty("describe")]
  public string Describe { get; set; }

  public ItemT() {
    this.Id = 0;
    this.Name = null;
    this.ItemType = Fantasy.Config.ItemType.Unknown;
    this.Describe = null;
  }
}


}
