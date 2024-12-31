// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/RequestResponse.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace UserAuthManagementService.Domain.Protos {

  /// <summary>Holder for reflection information generated from Protos/RequestResponse.proto</summary>
  public static partial class RequestResponseReflection {

    #region Descriptor
    /// <summary>File descriptor for Protos/RequestResponse.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static RequestResponseReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChxQcm90b3MvUmVxdWVzdFJlc3BvbnNlLnByb3RvEg9SZXF1ZXN0UmVzcG9u",
            "c2UaH2dvb2dsZS9wcm90b2J1Zi90aW1lc3RhbXAucHJvdG8idgoOUmVxdWVz",
            "dE1lc3NhZ2USEgoKbWV0aG9kTmFtZRgBIAEoCRIOCgZjYWxsZXIYAiABKAkS",
            "DwoHcGF5bG9hZBgDIAEoCRIvCgtyZXF1ZXN0VGltZRgEIAEoCzIaLmdvb2ds",
            "ZS5wcm90b2J1Zi5UaW1lc3RhbXAieAoPUmVzcG9uc2VNZXNzYWdlEg4KBmNh",
            "bGxlchgBIAEoCRISCgptZXRob2ROYW1lGAIgASgJEg8KB3BheWxvYWQYAyAB",
            "KAkSMAoMcmVzcG9uc2VUaW1lGAQgASgLMhouZ29vZ2xlLnByb3RvYnVmLlRp",
            "bWVzdGFtcDJjCg9SZXF1ZXN0TWVzc2FnZXISUAoLU2VuZFJlcXVlc3QSHy5S",
            "ZXF1ZXN0UmVzcG9uc2UuUmVxdWVzdE1lc3NhZ2UaIC5SZXF1ZXN0UmVzcG9u",
            "c2UuUmVzcG9uc2VNZXNzYWdlQiqqAidVc2VyQXV0aE1hbmFnZW1lbnRTZXJ2",
            "aWNlLkRvbWFpbi5Qcm90b3NiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::UserAuthManagementService.Domain.Protos.RequestMessage), global::UserAuthManagementService.Domain.Protos.RequestMessage.Parser, new[]{ "MethodName", "Caller", "Payload", "RequestTime" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::UserAuthManagementService.Domain.Protos.ResponseMessage), global::UserAuthManagementService.Domain.Protos.ResponseMessage.Parser, new[]{ "Caller", "MethodName", "Payload", "ResponseTime" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class RequestMessage : pb::IMessage<RequestMessage>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<RequestMessage> _parser = new pb::MessageParser<RequestMessage>(() => new RequestMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<RequestMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::UserAuthManagementService.Domain.Protos.RequestResponseReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestMessage(RequestMessage other) : this() {
      methodName_ = other.methodName_;
      caller_ = other.caller_;
      payload_ = other.payload_;
      requestTime_ = other.requestTime_ != null ? other.requestTime_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestMessage Clone() {
      return new RequestMessage(this);
    }

    /// <summary>Field number for the "methodName" field.</summary>
    public const int MethodNameFieldNumber = 1;
    private string methodName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string MethodName {
      get { return methodName_; }
      set {
        methodName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "caller" field.</summary>
    public const int CallerFieldNumber = 2;
    private string caller_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Caller {
      get { return caller_; }
      set {
        caller_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "payload" field.</summary>
    public const int PayloadFieldNumber = 3;
    private string payload_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Payload {
      get { return payload_; }
      set {
        payload_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "requestTime" field.</summary>
    public const int RequestTimeFieldNumber = 4;
    private global::Google.Protobuf.WellKnownTypes.Timestamp requestTime_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Google.Protobuf.WellKnownTypes.Timestamp RequestTime {
      get { return requestTime_; }
      set {
        requestTime_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as RequestMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(RequestMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MethodName != other.MethodName) return false;
      if (Caller != other.Caller) return false;
      if (Payload != other.Payload) return false;
      if (!object.Equals(RequestTime, other.RequestTime)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (MethodName.Length != 0) hash ^= MethodName.GetHashCode();
      if (Caller.Length != 0) hash ^= Caller.GetHashCode();
      if (Payload.Length != 0) hash ^= Payload.GetHashCode();
      if (requestTime_ != null) hash ^= RequestTime.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (MethodName.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(MethodName);
      }
      if (Caller.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Caller);
      }
      if (Payload.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Payload);
      }
      if (requestTime_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(RequestTime);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (MethodName.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(MethodName);
      }
      if (Caller.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Caller);
      }
      if (Payload.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Payload);
      }
      if (requestTime_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(RequestTime);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (MethodName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MethodName);
      }
      if (Caller.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Caller);
      }
      if (Payload.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Payload);
      }
      if (requestTime_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(RequestTime);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(RequestMessage other) {
      if (other == null) {
        return;
      }
      if (other.MethodName.Length != 0) {
        MethodName = other.MethodName;
      }
      if (other.Caller.Length != 0) {
        Caller = other.Caller;
      }
      if (other.Payload.Length != 0) {
        Payload = other.Payload;
      }
      if (other.requestTime_ != null) {
        if (requestTime_ == null) {
          RequestTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        RequestTime.MergeFrom(other.RequestTime);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            MethodName = input.ReadString();
            break;
          }
          case 18: {
            Caller = input.ReadString();
            break;
          }
          case 26: {
            Payload = input.ReadString();
            break;
          }
          case 34: {
            if (requestTime_ == null) {
              RequestTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(RequestTime);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            MethodName = input.ReadString();
            break;
          }
          case 18: {
            Caller = input.ReadString();
            break;
          }
          case 26: {
            Payload = input.ReadString();
            break;
          }
          case 34: {
            if (requestTime_ == null) {
              RequestTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(RequestTime);
            break;
          }
        }
      }
    }
    #endif

  }

  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class ResponseMessage : pb::IMessage<ResponseMessage>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<ResponseMessage> _parser = new pb::MessageParser<ResponseMessage>(() => new ResponseMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<ResponseMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::UserAuthManagementService.Domain.Protos.RequestResponseReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ResponseMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ResponseMessage(ResponseMessage other) : this() {
      caller_ = other.caller_;
      methodName_ = other.methodName_;
      payload_ = other.payload_;
      responseTime_ = other.responseTime_ != null ? other.responseTime_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ResponseMessage Clone() {
      return new ResponseMessage(this);
    }

    /// <summary>Field number for the "caller" field.</summary>
    public const int CallerFieldNumber = 1;
    private string caller_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Caller {
      get { return caller_; }
      set {
        caller_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "methodName" field.</summary>
    public const int MethodNameFieldNumber = 2;
    private string methodName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string MethodName {
      get { return methodName_; }
      set {
        methodName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "payload" field.</summary>
    public const int PayloadFieldNumber = 3;
    private string payload_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Payload {
      get { return payload_; }
      set {
        payload_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "responseTime" field.</summary>
    public const int ResponseTimeFieldNumber = 4;
    private global::Google.Protobuf.WellKnownTypes.Timestamp responseTime_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Google.Protobuf.WellKnownTypes.Timestamp ResponseTime {
      get { return responseTime_; }
      set {
        responseTime_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as ResponseMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(ResponseMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Caller != other.Caller) return false;
      if (MethodName != other.MethodName) return false;
      if (Payload != other.Payload) return false;
      if (!object.Equals(ResponseTime, other.ResponseTime)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Caller.Length != 0) hash ^= Caller.GetHashCode();
      if (MethodName.Length != 0) hash ^= MethodName.GetHashCode();
      if (Payload.Length != 0) hash ^= Payload.GetHashCode();
      if (responseTime_ != null) hash ^= ResponseTime.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Caller.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Caller);
      }
      if (MethodName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(MethodName);
      }
      if (Payload.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Payload);
      }
      if (responseTime_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(ResponseTime);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Caller.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Caller);
      }
      if (MethodName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(MethodName);
      }
      if (Payload.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Payload);
      }
      if (responseTime_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(ResponseTime);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Caller.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Caller);
      }
      if (MethodName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MethodName);
      }
      if (Payload.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Payload);
      }
      if (responseTime_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(ResponseTime);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(ResponseMessage other) {
      if (other == null) {
        return;
      }
      if (other.Caller.Length != 0) {
        Caller = other.Caller;
      }
      if (other.MethodName.Length != 0) {
        MethodName = other.MethodName;
      }
      if (other.Payload.Length != 0) {
        Payload = other.Payload;
      }
      if (other.responseTime_ != null) {
        if (responseTime_ == null) {
          ResponseTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        ResponseTime.MergeFrom(other.ResponseTime);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Caller = input.ReadString();
            break;
          }
          case 18: {
            MethodName = input.ReadString();
            break;
          }
          case 26: {
            Payload = input.ReadString();
            break;
          }
          case 34: {
            if (responseTime_ == null) {
              ResponseTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(ResponseTime);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Caller = input.ReadString();
            break;
          }
          case 18: {
            MethodName = input.ReadString();
            break;
          }
          case 26: {
            Payload = input.ReadString();
            break;
          }
          case 34: {
            if (responseTime_ == null) {
              ResponseTime = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(ResponseTime);
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
