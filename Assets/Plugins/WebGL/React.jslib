mergeInto(LibraryManager.library, {
  GameObjectDataRecordingDone: function (dataJson) {
    try {
      window.dispatchReactUnityEvent("GameObjectDataRecordingDone", UTF8ToString(dataJson));
    } catch (e) {
      console.warn("Failed to dispatch event");
    }
  },
});