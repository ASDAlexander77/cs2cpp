#traverse all the directory and subdirectory
define walk
  $(wildcard $(1)) $(foreach e, $(wildcard $(1)/*), $(call walk, $(e)))
endef

LOCAL_PATH := $(call my-dir)
SRC_PATH := $(LOCAL_PATH)/../../src $(LOCAL_PATH)/../../Impl

include $(CLEAR_VARS)

LOCAL_MODULE    := CoreLib

ALLFILES = $(call walk, $(SRC_PATH))
FILE_LIST := $(filter %.cpp, $(ALLFILES))

LOCAL_SRC_FILES := $(FILE_LIST:$(LOCAL_PATH)/%=%)

LOCAL_C_INCLUDES := $(SRC_PATH) $(LOCAL_PATH)/../../bdwgc/include
				   
LOCAL_CPPFLAGS += -Wno-write-strings -Wno-conversion-null -Wno-invalid-offsetof -fpermissive

include $(BUILD_STATIC_LIBRARY)
