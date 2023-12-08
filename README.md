![Keyboard Inspector](https://github.com/mat1jaczyyy/Keyboard-Inspector/assets/13300194/d9647439-2e20-4c43-9158-3a251cae4a40)

Keyboard Inspector is a standalone tool for recording and analyzing input data captured from keyboards, joysticks/gamepads, and mice. It is essential for researchers, developers and gamers looking to empirically evaluate the internal workings of their input hardware.

## Getting Started

Download Keyboard Inspector from the [Releases](/releases) page, and extract to a folder of your choosing. Keyboard Inspector is a fully portable Windows application and no installation variant is offered.

Begin by using the Start/Stop Recording button in the top-right corner of the user interface to start recording your inputs for analysis. Once stopped, results of the analysis will be shown immediately. The analysis interface has three main parts. 

At the top, three graphs show different methods of interpreting the data in the time domain by binning the timestamps of input events into histograms:

- *Differences between consecutive input events*. The histogram shows how often a given timestamp difference is observed between consecutive events in the data.

- *Differences between all input events* no further than one second apart. The histogram shows how often a given timestamp difference is observed between any pair of events closer than one second.

- Timestamps of input events modulo one second. The histogram shows how often a given decimal part is observed within the timestamps, effectively *wrapping the timestamps around a single second*.

Three more graphs are displayed under showing the frequency domains of these histograms by applying the discrete Fourier transform. Interpreting these can a be a little tricky and although not always the case, **the largest peak is generally the effective report rate of the device**. In most cases, this will coincide with the device's USB report rate.

If the data is too noisy or has multiple peaks, recording more input data helps with increasing the sample size. Applying iterations of HPS partial elimination can help isolate the fundamental peak. If no clear peak presents itself, try increasing the binning rate, as it might be outside the range you're analyzing. 

The binning rate is set to 4000 Hz by default, which will let you comfortably analyze full speed USB devices (which support up to 1000 Hz). Increase this value if you need to analyze higher report rate devices such as high speed USB devices (which support up to 8000 Hz).

To specifically check for chord splitting behavior, intentionally try to press multiple keys simultaneously during recording, and then observe the value of 0 ms on the difference histograms. If the keyboard does not chord split you will see a large peak here, however if it does you will see a flat zero.

As recordings contain your inputs, **be vary of sharing any recordings which might contain sensitive information such as personal messages, identifiable information or passwords**.

Finally, the timeline at the bottom is simply a visual diagram of all inputs recorded grouped by device. Specific keys and entire devices can be excluded from the analysis here.

## Help and Support

For more detailed information and additional functionality, read the [documentation](/doc).

If you need help interpreting analysis results, or have general feedback or criticism, join our [Discord server](https://discord.gg/kX4cJQH5Zn).

## Special thanks

Special thanks to the following contributors for helping make Keyboard Inspector possible:
- [Zepheniah](https://github.com/Zepheniahh) for contributing the core idea for Differences between all events, endless feedback and for keeping me sane during development.
- [Spazza17](https://www.youtube.com/channel/UC4JEl2dMSkFUCXnYvh1lIzw) for initially inspiring me to develop this tool in the first place.
- TETR.IO player [FURRY](https://docs.google.com/document/d/1bfQFBUv85jFLSLyiyCotMBU19xeUtQb3wUEas7Zfq_Y/edit?usp=sharing) for incentivizing me to develop Fourier analysis techniques to detect sampling rates.
- Clone Hero community for the existence of quad/quint zigs and their obsession with overtap.
- all other early testers.
