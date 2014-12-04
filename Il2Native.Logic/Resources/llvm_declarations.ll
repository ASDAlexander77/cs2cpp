declare i8* @malloc(i32) #99900

; Function Attrs: nounwind
;declare void @llvm.memcpy.p0i8.p0i8.i32(i8* nocapture, i8* nocapture readonly, i32, i32, i1) #88801
declare void @llvm.memcpy.p0i8.p0i8.i32(i8* nocapture, i8* nocapture, i32, i32, i1) #88801
declare void @llvm.memset.p0i8.i32(i8*, i8, i32, i32, i1) #88802

declare { i8, i1 } @llvm.sadd.with.overflow.i8(i8, i8) #0
declare { i16, i1 } @llvm.sadd.with.overflow.i16(i16, i16) #0
declare { i32, i1 } @llvm.sadd.with.overflow.i32(i32, i32) #0
declare { i64, i1 } @llvm.sadd.with.overflow.i64(i64, i64) #0

declare { i8, i1 } @llvm.ssub.with.overflow.i8(i8, i8) #0
declare { i16, i1 } @llvm.ssub.with.overflow.i16(i16, i16) #0
declare { i32, i1 } @llvm.ssub.with.overflow.i32(i32, i32) #0
declare { i64, i1 } @llvm.ssub.with.overflow.i64(i64, i64) #0

declare { i8, i1 } @llvm.smul.with.overflow.i8(i8, i8) #0
declare { i16, i1 } @llvm.smul.with.overflow.i16(i16, i16) #0
declare { i32, i1 } @llvm.smul.with.overflow.i32(i32, i32) #0
declare { i64, i1 } @llvm.smul.with.overflow.i64(i64, i64) #0

attributes #88801 = { nounwind }
attributes #88802 = { nounwind }

; Exception support - DWARF
declare i32 @__gxx_personality_v0(...) #78801
declare i8* @__cxa_allocate_exception(i32) #78802
declare void @__cxa_free_exception(i8*) #78803
declare void @__cxa_throw(i8*, i8*, i8*) #78804
; Function Attrs: nounwind readnone
declare i32 @llvm.eh.typeid.for(i8*) #78805
declare i8* @__cxa_begin_catch(i8*) #78806
declare void @__cxa_end_catch() #78807
declare void @__cxa_call_unexpected(i8*) #78808
declare void @__cxa_rethrow() #78809
; Function Attrs: nounwind readonly
declare i8* @__dynamic_cast(i8*, i8*, i8*, i32) #78810
; 'c++ new' Function Attrs: nobuiltin
declare noalias i8* @_Znwj(i32) #78811
; 'c++ delete' Function Attrs: nobuiltin nounwind
declare void @_ZdlPv(i8*) #78812
declare void @__cxa_pure_virtual() #78813

attributes #78805 = { nounwind readnone }
attributes #78810 = { nounwind readonly }
attributes #78811 = { nobuiltin }
attributes #78812 = { nobuiltin nounwind }

; RTTI externals
@_ZTVN10__cxxabiv116__enum_type_infoE = external global i8*
@_ZTVN10__cxxabiv117__array_type_infoE = external global i8*
@_ZTVN10__cxxabiv117__class_type_infoE = external global i8*
@_ZTVN10__cxxabiv119__pointer_type_infoE = external global i8*
@_ZTVN10__cxxabiv120__si_class_type_infoE = external global i8*
@_ZTVN10__cxxabiv120__function_type_infoE = external global i8*
@_ZTVN10__cxxabiv121__vmi_class_type_infoE = external global i8*
@_ZTVN10__cxxabiv123__fundamental_type_infoE = external global i8*
@_ZTVN10__cxxabiv129__pointer_to_member_type_infoE = external global i8*
