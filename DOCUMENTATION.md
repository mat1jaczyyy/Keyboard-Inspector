## Recording and viewing input data

Inputs are listened to using the Win32 [Raw Input](https://learn.microsoft.com/en-us/windows/win32/inputdev/about-raw-input) API, and timestamped using the [highest resolution timer](https://learn.microsoft.com/en-us/windows/win32/api/profileapi/nf-profileapi-queryperformancecounter) available on the machine at the time of processing the received input (generally 0.1μs). This relies on being able to get CPU time as soon as the input arrives, so high CPU utilization by other processes can compromise affect the recording negatively.

Currently, input listening and timestamping is done on the same thread as the user interface, which is why it has to remain empty and static in order to not further affect the recording negatively. In the future, we hope to offload this work to a separate thread so the analysis results can be rendered in the user interface in real-time.

Use the Start/Stop Recording button in the top-right corner of the user interface to start recording your inputs for analysis. Next to the button, you can find recording-time settings:

- *Capture* - allows you to select which kinds of input devices you want to listen to: keyboards, joysticks/gamepads, or mice. Listening to mouse input is disabled by default as the movement data is very loud, and we currently only support capturing button states.

- *Freeze* - when doing multiple recordings in succession, checking this option will ensure the inputs which are currently visible on the Timeline will be kept ("frozen") for the new recording. The next section explains how to select inputs for freezing in further detail.

After stopping the recording, results of the analysis will be shown immediately. You may now observe the Timeline representing the recorded data as well as the analysis results.

### Input Timeline

The Timeline spans the bottom of the user interface. It is a visual diagram of all recorded input events. The amount of input events in the recording (sample size) is displayed top-right of the Timeline.

To move or zoom the viewport, click and drag with your mouse, use the mouse wheel or use the scroll bar below the Timeline. Hovering your mouse cursor over a specific input event will display the exact timestamp of the event.

Each input is listed on the left hand side within its own row. You can rearrange the inputs by dragging them around. If there are multiple devices, inputs will be grouped by device. The default sorting can be restored by right clicking anywhere within the Timeline and selecting *Sort by Device*.

Right click anywhere within an input's row to bring up a context menu which lets you change the color of the input's events, or hide the input which will *exclude it from the analysis*. Hidden inputs can be restored by right clicking anywhere within the Timeline and selecting *Show All Inputs*. Only the currently visible inputs will be selected for freezing, as explained in the previous section.

Right click a device name to bring up a context menu which lets you change the color of all of the device's events, or hide the device which will exclude all of its inputs from the analysis. If you additionally hold the Shift key while right-clicking, an additional option to copy the device's [interface name](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdeviceinfoa#parameters) shows (generally contains the device's USB Vendor and Product IDs).

## Analyzing input data

The analysis portion of the interface is divided into a 3x2 grid of various useful charts. The first row contains time domain charts, and the second row contains frequency domain charts. Each column represents a different method of analyzing the data. These charts' viewports can be interacted with similarly to the Timeline. Double click any chart to enlarge it. Below, some controls are available to tweak the analysis parameters.

### Time domain

The recorded input timestamps from the non-hidden portion of the Timeline are interpreted for analysis by binning them into histograms. The resulting time domain charts are displayed in the first row of the grid. Three different methods of binning are available, each represented by a different column:

- *Differences between consecutive input events* - First, a list of differences between the timestamps of consecutive of input events is calculated. The amount of information scales linearly with the amount of input events recorded. Then, these differences are binned into a histogram. The histogram shows how often a given timestamp difference is observed between consecutive events in the data. 

- *Differences between all input events* - First, a list of all differences between the timestamps of all input events is calculated. As the amount of information scales quadratically with the amount of input events recorded, differences larger than one second are discarded due to memory and processing concerns. Then, these differences are binned into a histogram. The histogram shows how often a given timestamp difference is observed between all events in the data.

- *Events wrapped around a second* - First, the timestamps of all input events are modulo'd with one second, effectively keeping only the decimal part. The amount of information scales linearly with the amount of input events recorded. Then, these timestamps are binned into a histogram. The histogram shows how often a given decimal part is observed within the timestamps, effectively wrapping the timestamps around a single second.

The time domain charts are useful for checking if certain timings are being treated unfairly by the device. For example, some devices are incapable of reporting multiple input events simultaneously (commonly known as *chord splitting*). These devices will lack a peak at 0 ms in the difference histograms.

### Frequency domain

Next, each histogram is transformed into the frequency domain by applying the [discrete Fourier transform](https://www.fftw.org/fftw3_doc/One_002dDimensional-DFTs-of-Real-Data.html). The resulting frequency domain data is complex and the phase of the complex values is irrelevant, so their absolute values are displayed in the frequency domain charts in the second row of the grid.

A physical state change of a key switch usually goes through multiple different processing steps before reaching a Windows application: microcontroller key scanning (optionally with a debounce filter), USB reporting, computer OS input handling, task switching time, application response time. While the OS time can be treated as unpredictable continuous noise (as Windows is non-RTOS), the former processes have very strict sampling rates.

The frequency domain is useful for detecting periodicity in the data, such as the key scanning rate and/or USB report rate. If the data is too noisy, increase the sample size by recording more data.

### Analysis parameters

The *Binning Rate* is set to 4000 Hz by default, which will let you comfortably analyze full speed USB devices (which support up to 1000 Hz). Increase this value if you need to analyze higher report rate devices such as high speed USB devices (which support up to 8000 Hz). Read more about USB endpoint intervals [here](https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/usbspec/ns-usbspec-_usb_endpoint_descriptor#members).

The *Low Cut Filter* cuts out frequencies below ~50 Hz, which usually represent the raised floor of the time domain histograms (all the values are greater than or equal to zero). Devices typically exhibit higher sampling rates than this. Uncheck if analyzing a device with an extremely low effective sampling rate.

Higher frequency harmonics will appear at multiples of the device sampling rate's fundamental frequency. This is because it's still possible for higher sampling rates to have generated the recorded data, albeit less likely. The [Harmonic Product Spectrum](http://musicweb.ucsd.edu/~trsmyth/analysis/Harmonic_Product_Spectrum.html) algorithm eliminates these harmonics by multiplying together a few iterations of downsampled frequency domain data. These line up in such a way that the largest product always corresponds to the fundamental frequency. This feature is most useful in situations where the fundamental frequency isn't obvious due to excessive noise.

## Working with files

After recording, the captured data only exists inside the program's memory. The recording can be saved into a file using the *File* menu in the top-left corner of the user interface. Stored recordings can then be shared with others and be opened from the same menu, either from disk or from an URL.

Keyboard Inspector files (`*.kbi`) use a custom binary format which contains the version of the program which created the recording, date and time of the recording, information about physical devices whose inputs were captured, the captured inputs themselves, and the current analysis parameters. No other fingerprinting or identifiable information is included.

As recordings contain your inputs, **be vary of sharing any recordings which might contain sensitive information**.

### Importing from other replay formats

The *File* menu can also open replay formats other than Keyboard Inspector files. These are generally replays from various games that contain raw input timestamps which can be analyzed:

#### TETR.IO

[TETR.IO](https://tetr.io) is a free-to-win modern yet familiar online stacker by osk. The input data inside TETR.IO replays (`*.ttr`) is not very accurate due to [browser security requirements](https://developer.mozilla.org/en-US/docs/Web/API/Performance/now#security_requirements) and due to TETR.IO downsampling input timestamps to subframes before utilizing them in the game engine and subsequently in the replay.

TETR.IO's engine runs at 60 frames per second, and each frame contains 10 subframes an input could land on, ultimately acting as an additional 600 Hz sampling rate. This is unlike regular Keyboard Inspector recordings which try to get the most accurate real-time timestamp it can without any additional downsampling. This means it is only [practical](https://docs.google.com/document/d/1bfQFBUv85jFLSLyiyCotMBU19xeUtQb3wUEas7Zfq_Y/edit#heading=h.4rs4l97dqr05) to analyze TETR.IO replays if you're looking for device sampling rates under 300 Hz.

If you're loading a multiplayer replay file (`*.ttrm`), you will additionally have to select which specific replay from the file to load considering there are separate replays for each player and each game played in the set.

If you're importing from URL, you can paste a live TETR.IO replay link and Keyboard Inspector will pull it from the TETR.IO servers.
