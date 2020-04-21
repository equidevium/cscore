﻿using com.csutil.ui;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace com.csutil {

    public static class AppFlowUnityExtensions {

        public static IAppFlow WithAllTrackingActive(this IAppFlow self) {
            self.ActivateLinkMapTracking();
            self.ActivatePrefabLoadTracking();
            self.ActivateUiEventTracking();
            self.ActivateViewStackTracking();
            return self;
        }

        public static void ActivatePrefabLoadTracking(this IAppFlow self) {
            EventBus.instance.Subscribe(self, EventConsts.catTemplate, (GameObject prefab) => {
                self.TrackEvent(EventConsts.catTemplate, "Loaded_" + prefab.name, prefab);
            });
        }

        public static void ActivateUiEventTracking(this IAppFlow self) {

            // Button UI tracking:
            EventBus.instance.Subscribe(self, UiEvents.BUTTON_CLICKED, (Button button) => {
                self.TrackEvent(EventConsts.catUi, UiEvents.BUTTON_CLICKED + "_" + button, button);
            });

            // Toggle UI tracking:
            EventBus.instance.Subscribe(self, UiEvents.TOGGLE_CHANGED, (Toggle toggle, bool isChecked) => {
                self.TrackEvent(EventConsts.catUi, UiEvents.TOGGLE_CHANGED + "_" + toggle + "_" + isChecked, toggle, isChecked);
            });

            // InputField UI tracking:
            EventHandler<string> action = (input, newText) => {
                self.TrackEvent(EventConsts.catUi, UiEvents.INPUTFIELD_CHANGED + "_" + input, input);
            };
            var delayedAction = action.AsThrottledDebounce(delayInMs: 1900, skipFirstEvent: true);
            EventBus.instance.Subscribe(self, UiEvents.INPUTFIELD_CHANGED, (InputField input, string newText) => {
                delayedAction(input, newText);
            });
        }

        public static void ActivateViewStackTracking(this IAppFlow self) {
            EventBus.instance.Subscribe(self, EventConsts.VIEW_SHOW, (GameObject view) => {
                self.TrackEvent(EventConsts.catView, EventConsts.VIEW_SHOW + "_" + view.name, view);
            });
            EventBus.instance.Subscribe(self, EventConsts.VIEW_SWITCH_BACK_TO_LAST, (string currentViewName, GameObject lastView) => {
                self.TrackEvent(EventConsts.catView, EventConsts.VIEW_SWITCH_BACK_TO_LAST + "_" + currentViewName + "->" + lastView.name, lastView);
            });
            EventBus.instance.Subscribe(self, EventConsts.VIEW_SWITCH_TO_NEXT, (GameObject currentView, GameObject nextView) => {
                self.TrackEvent(EventConsts.catView, EventConsts.VIEW_SWITCH_TO_NEXT + "_" + currentView.name + "->" + nextView.name, currentView, nextView);
            });
            EventBus.instance.Subscribe(self, EventConsts.VIEW_ADDED, (GameObject view) => {
                self.TrackEvent(EventConsts.catView, EventConsts.VIEW_ADDED + "_" + view.name, view);
            });
            EventBus.instance.Subscribe(self, EventConsts.VIEW_REMOVED, (GameObject view) => {
                self.TrackEvent(EventConsts.catView, EventConsts.VIEW_REMOVED + "_" + view.name, view);
            });
        }

    }

}
