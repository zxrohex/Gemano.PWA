# Gemano.PWA
A Progressive Web App (PWA) for Chrome's experimental local on-device Gemini Nano APIs,
mostly made for testing and experimenting with the APIs (and to finally ship an Blazor project).

Currently majorily targeted on being an Chat/Assistant app with some additional features, with more implementations
of the local on-device AI APIs in the future (Translation, Re-/Write, Summarization, Language Detection).

Everything is written from the ground-up by me (referring to UI/UX and JavaScript implementations/interfaces).
I am trying to not rely on UI/UX libraries and frameworks at all - I want to create and design the entire UI/UX by myself.

Why do the entire UI/UX stuff alone from zero? To learn and improve my skills in UI/UX design and development, and because it's what I primarily did and want to do - and because it's fun.

> [!NOTE]
> I am using SCSS instead of vanilla CSS with Prepros as a utility (and for compiling Sass/SCSS files).

## Before you start
This will **only** run on the latest Chrome Canary version (**please** use 134.0.6949.0 or newer) with the following flags enabled:
- `chrome://flags/#prompt-api-for-gemini-nano` set to `Enabled`
- `chrome://flags/#optimization-guide-on-device-model` set to `Enabled BypassPerfRequirement`
- `chrome://flags/#text-safety-classifier` set to `Disabled`
(for now or else it will act like an "Family Friendly"" YouTube channel aka it refuses to respond to mostly everything, it's a known bug)

**It will not work in the following scenarios:**
- Inside Incognito and/or Guest mode
- On an Enterprise-managed instance with `GenAILocalFoundationalModelSettings` set to "Do not download model".
Check by going to `chrome://policy` and search for `GenAILocalFoundationalModelSettings`

Make sure you have **at least**
- 6 GB VRAM (iGPU or dGPU, doesn't matter)
- 22 GB (more than 50 GB is urgently recommended, see info below) of free storage on the volume containing your Chrome profile


> [!WARNING]
> If the available storage space falls below 10 GB, the model will be automatically deleted and you will
> have to download it again.

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
This will log the progress of the download in the console and will quickly make Chrome unusable due
to the amount of entries it will create.

Try to keep Chrome open and running because if you close it in midst of the download, the download on the next start
will be acting weirdly and may take longer to finish.

## Reporting model/API-related issues
This is only relevant if you tried EVERYTHING and ANYTHING possible with all requirements met and all setup done correctly, or if the issues or bugs
are related to Chrome and/or the APIs/components themselves.

Do the following steps to report an issue to the Chromium devs themselves:

1. Open a new tab  
2. Go to chrome://gpu  
3. Download the gpu report  
4. Go to chrome://histograms/\#OptimizationGuide.ModelExecution.OnDeviceModelInstallCriteria.AtRegistration.DiskSpace  
    * If you see records for 0, it means that your device doesn’t have enough storage space for the model. Ensure that you have at least 22 GB on the disk with your user profile, and retry the [setup steps](#heading=h.witohboigk0o). If you are still stuck, continue with the other steps below.  
      ![image1](https://private-user-images.githubusercontent.com/108447422/402179406-bfac5d88-2724-4bf9-b3a0-b9bb0453e0ea.png?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3MzY1NTQxMDYsIm5iZiI6MTczNjU1MzgwNiwicGF0aCI6Ii8xMDg0NDc0MjIvNDAyMTc5NDA2LWJmYWM1ZDg4LTI3MjQtNGJmOS1iM2EwLWI5YmIwNDUzZTBlYS5wbmc_WC1BbXotQWxnb3JpdGhtPUFXUzQtSE1BQy1TSEEyNTYmWC1BbXotQ3JlZGVudGlhbD1BS0lBVkNPRFlMU0E1M1BRSzRaQSUyRjIwMjUwMTExJTJGdXMtZWFzdC0xJTJGczMlMkZhd3M0X3JlcXVlc3QmWC1BbXotRGF0ZT0yMDI1MDExMVQwMDAzMjZaJlgtQW16LUV4cGlyZXM9MzAwJlgtQW16LVNpZ25hdHVyZT1hZDE2YzY1OGQ1Zjk2MGQwODIwOGRkYTBjMzg1MDdlNzY4NmYwN2ZmZTk0MDk0ODlmMzdiYzVjN2FmNjhkZjdkJlgtQW16LVNpZ25lZEhlYWRlcnM9aG9zdCJ9.dzF2QlQEmu2Kb6p667luYq112vcu-tzb1Ac2TNJ-g8c)
      On qualifying systems, the histogram should look similar to the following example:\
      `- Histogram: OptimizationGuide.ModelExecution.OnDeviceModelInstallCriteria.AtRegistration.DiskSpace recorded 1 samples, mean = 1.0 (flags = 0x41) [#]`
5. Go to chrome://histograms/\#OptimizationGuide and download the histograms report
6. File an report at the [Chromium Issue Tracker](https://issues.chromium.org/u/0/issues/new?component=1583624&template=0&pli=1)


## Reporting issues and bugs
Error-handling is nearly non-existent currently (will change soon once most of the features and functions are implemented),
so if you encounter any issues or bugs (you will get an error message at the bottom of the page), open the DevTools console
and copy everything that is in there and paste it in a new issue. I will try to fix it as soon as possible.

## Link to CI/CD deployment (the actual web app)
[https://zxrohex.github.io/Gemano.PWA/](https://zxrohex.github.io/Gemano.PWA/)

## Changelog
### commit e319bc80f6a424bd73e68d978e506b2a3b017790 (11.01.2025)
- Chat history (read-only)
- Polishing / improvements on currently used UI components
- Started work on new UI components

### commit 2a92141c308d926e81f238c70b428f07a781169d (28.12.2024)
- Initial working public release from CI/CD

### commit 0224087112b9e18b2ccbad4cb9e366f428d169bd (28.12.2024)
- Initial commit
