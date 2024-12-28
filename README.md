# Gemano.PWA
A Progressive Web App (PWA) for Chrome's experimental local on-device Gemini Nano APIs,
mostly made for testing and experimenting with the APIs (and to finally ship an Blazor project).

## !! NOTE !! 
This will **only** run on the latest Chrome Canary version with the following flags enabled:
- `chrome://flags/#prompt-api-for-gemini-nano` set to `Enabled`
- `chrome://flags/#optimization-guide-on-device-model` set to `Enabled BypassPerfRequirement`
- `chrome://flags/#text-safety-classifier` set to `Disabled`
(for now or else it will act like an "Family Friendly"" YouTube channel aka it refuses to respond to mostly everything, it's a known bug)

If you have done everything correctly, open a DevTools console on a page and run the following code:

```javascript
(await ai.languageModel.capabilities()).available
```
It will say "no" or "after-download". In both cases, run the following code:
```javascript
await ai.languageModel.create();
```
Now it will throw an error, but that's supposed to happen. It should download the model
in the background now - you can check if it does by looking in the Task Manager or by
going to `chrome://components` and check for something called `Optimization Guide On Device Model`.
It will display the status of the component, and additionally, if running the JavaScript code
didn't trigger the download of the model, you can manually trigger it by clicking on "Check for updates".
Then just wait until it says something like "Up to date" or until resource usage from Chrome Canary calms down.
If all of this didn't work then I don't know man, I'm just a developer.

You can also actually log the progress of the download by running the following code:
```javascript
await ai.languageModel.create({
  monitor(m) {
    m.addEventListener("downloadprogress", e => {
      console.log(`Downloaded ${e.loaded} of ${e.total} bytes.`);
    });
  }
});
```
This will log the progress of the download in the console and will quickly make your Chrome unusable due
to the amount of logs it will generate.

## Link to CI/CD deployment (the actual web app)
[https://zxrohex.github.io/Gemano.PWA/](https://zxrohex.github.io/Gemano.PWA/)