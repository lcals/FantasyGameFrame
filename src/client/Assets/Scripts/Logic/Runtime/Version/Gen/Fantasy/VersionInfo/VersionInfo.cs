// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Fantasy.VersionInfo
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

/// VersionInfo
public struct VersionInfo : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static VersionInfo GetRootAsVersionInfo(ByteBuffer _bb) { return GetRootAsVersionInfo(_bb, new VersionInfo()); }
  public static VersionInfo GetRootAsVersionInfo(ByteBuffer _bb, VersionInfo obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public VersionInfo __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  /// url
  public string Url { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetUrlBytes() { return __p.__vector_as_span<byte>(4, 1); }
#else
  public ArraySegment<byte>? GetUrlBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetUrlArray() { return __p.__vector_as_array<byte>(4); }
  /// update data 
  public bool Update { get { int o = __p.__offset(6); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }
  /// update data 
  public int DataVersion { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  /// update asset 
  public int AssetVersion { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  /// total version
  public int TotalVersion { get { int o = __p.__offset(12); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  /// server ip
  public string ServerIp { get { int o = __p.__offset(14); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetServerIpBytes() { return __p.__vector_as_span<byte>(14, 1); }
#else
  public ArraySegment<byte>? GetServerIpBytes() { return __p.__vector_as_arraysegment(14); }
#endif
  public byte[] GetServerIpArray() { return __p.__vector_as_array<byte>(14); }
  /// output time
  public string OutputTime { get { int o = __p.__offset(16); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetOutputTimeBytes() { return __p.__vector_as_span<byte>(16, 1); }
#else
  public ArraySegment<byte>? GetOutputTimeBytes() { return __p.__vector_as_arraysegment(16); }
#endif
  public byte[] GetOutputTimeArray() { return __p.__vector_as_array<byte>(16); }

  public static Offset<Fantasy.VersionInfo.VersionInfo> CreateVersionInfo(FlatBufferBuilder builder,
      StringOffset urlOffset = default(StringOffset),
      bool update = false,
      int data_version = 0,
      int asset_version = 0,
      int total_version = 0,
      StringOffset server_ipOffset = default(StringOffset),
      StringOffset output_timeOffset = default(StringOffset)) {
    builder.StartTable(7);
    VersionInfo.AddOutputTime(builder, output_timeOffset);
    VersionInfo.AddServerIp(builder, server_ipOffset);
    VersionInfo.AddTotalVersion(builder, total_version);
    VersionInfo.AddAssetVersion(builder, asset_version);
    VersionInfo.AddDataVersion(builder, data_version);
    VersionInfo.AddUrl(builder, urlOffset);
    VersionInfo.AddUpdate(builder, update);
    return VersionInfo.EndVersionInfo(builder);
  }

  public static void StartVersionInfo(FlatBufferBuilder builder) { builder.StartTable(7); }
  public static void AddUrl(FlatBufferBuilder builder, StringOffset urlOffset) { builder.AddOffset(0, urlOffset.Value, 0); }
  public static void AddUpdate(FlatBufferBuilder builder, bool update) { builder.AddBool(1, update, false); }
  public static void AddDataVersion(FlatBufferBuilder builder, int dataVersion) { builder.AddInt(2, dataVersion, 0); }
  public static void AddAssetVersion(FlatBufferBuilder builder, int assetVersion) { builder.AddInt(3, assetVersion, 0); }
  public static void AddTotalVersion(FlatBufferBuilder builder, int totalVersion) { builder.AddInt(4, totalVersion, 0); }
  public static void AddServerIp(FlatBufferBuilder builder, StringOffset serverIpOffset) { builder.AddOffset(5, serverIpOffset.Value, 0); }
  public static void AddOutputTime(FlatBufferBuilder builder, StringOffset outputTimeOffset) { builder.AddOffset(6, outputTimeOffset.Value, 0); }
  public static Offset<Fantasy.VersionInfo.VersionInfo> EndVersionInfo(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<Fantasy.VersionInfo.VersionInfo>(o);
  }
  public static void FinishVersionInfoBuffer(FlatBufferBuilder builder, Offset<Fantasy.VersionInfo.VersionInfo> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedVersionInfoBuffer(FlatBufferBuilder builder, Offset<Fantasy.VersionInfo.VersionInfo> offset) { builder.FinishSizePrefixed(offset.Value); }
  public VersionInfoT UnPack() {
    var _o = new VersionInfoT();
    this.UnPackTo(_o);
    return _o;
  }
  public void UnPackTo(VersionInfoT _o) {
    _o.Url = this.Url;
    _o.Update = this.Update;
    _o.DataVersion = this.DataVersion;
    _o.AssetVersion = this.AssetVersion;
    _o.TotalVersion = this.TotalVersion;
    _o.ServerIp = this.ServerIp;
    _o.OutputTime = this.OutputTime;
  }
  public static Offset<Fantasy.VersionInfo.VersionInfo> Pack(FlatBufferBuilder builder, VersionInfoT _o) {
    if (_o == null) return default(Offset<Fantasy.VersionInfo.VersionInfo>);
    var _url = _o.Url == null ? default(StringOffset) : builder.CreateString(_o.Url);
    var _server_ip = _o.ServerIp == null ? default(StringOffset) : builder.CreateString(_o.ServerIp);
    var _output_time = _o.OutputTime == null ? default(StringOffset) : builder.CreateString(_o.OutputTime);
    return CreateVersionInfo(
      builder,
      _url,
      _o.Update,
      _o.DataVersion,
      _o.AssetVersion,
      _o.TotalVersion,
      _server_ip,
      _output_time);
  }
}

public class VersionInfoT
{
  [Newtonsoft.Json.JsonProperty("url")]
  public string Url { get; set; }
  [Newtonsoft.Json.JsonProperty("update")]
  public bool Update { get; set; }
  [Newtonsoft.Json.JsonProperty("data_version")]
  public int DataVersion { get; set; }
  [Newtonsoft.Json.JsonProperty("asset_version")]
  public int AssetVersion { get; set; }
  [Newtonsoft.Json.JsonProperty("total_version")]
  public int TotalVersion { get; set; }
  [Newtonsoft.Json.JsonProperty("server_ip")]
  public string ServerIp { get; set; }
  [Newtonsoft.Json.JsonProperty("output_time")]
  public string OutputTime { get; set; }

  public VersionInfoT() {
    this.Url = null;
    this.Update = false;
    this.DataVersion = 0;
    this.AssetVersion = 0;
    this.TotalVersion = 0;
    this.ServerIp = null;
    this.OutputTime = null;
  }

  public static VersionInfoT DeserializeFromJson(string jsonText) {
    return Newtonsoft.Json.JsonConvert.DeserializeObject<VersionInfoT>(jsonText);
  }
  public string SerializeToJson() {
    return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
  }
  public static VersionInfoT DeserializeFromBinary(byte[] fbBuffer) {
    return VersionInfo.GetRootAsVersionInfo(new ByteBuffer(fbBuffer)).UnPack();
  }
  public byte[] SerializeToBinary() {
    var fbb = new FlatBufferBuilder(0x10000);
    VersionInfo.FinishVersionInfoBuffer(fbb, VersionInfo.Pack(fbb, this));
    return fbb.DataBuffer.ToSizedArray();
  }
}


}
