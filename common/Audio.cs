using NAudio.Wave;

namespace nlcEngine;

/// <summary>
/// Provides an OpenAL audio.
/// </summary>
public sealed class Audio : IDisposable
{
    private bool _disposed = false;
    private int _source;
    private int _buffer;
    private bool _playing = false;
    private float _volume = 1f;

    private static ALDevice Device;
    private static ALContext Context;

    /// <summary>
    /// Gets or sets the volume of the audio.
    /// </summary>
    public float Volume
    {
        get => _volume;
        set => _volume = Math.Clamp(value, 0f, 1f);
    }

    /// <summary>
    /// Gets whether the audio is currently being played.
    /// </summary>
    public bool IsPlaying
    {
        get => _playing;
    }

    private Audio()
    {

    }

    private Audio(byte[] pcm, ALFormat format, int size, int freq )
    {
        ALC.MakeContextCurrent(Context);

        _source = AL.GenSource();
        _buffer = AL.GenBuffer();

        AL.BufferData(_buffer, format, ref pcm[0], pcm.Length, freq);

        AL.BindBufferToSource(_source, _buffer);

        ResourceCollector.Add(this);
    }

    private static void InitContext()
    {
        Device = ALC.OpenDevice(null);
        Context = ALC.CreateContext(Device, (int[])null);

        NlcEngineGame.EnsureRelease(() =>
        {
            ALC.MakeContextCurrent(ALContext.Null);
            ALC.DestroyContext(Context);
            ALC.CloseDevice(Device);
        });
    }

    /// <summary>
    /// Loads the wave file.
    /// </summary>
    /// <param name="filename">file name</param>
    /// <returns>loaded audio</returns>
    public static Audio LoadWave( string filename )
    {
        NlcHelper.FileThrow(filename);

        if (Device == ALDevice.Null)
        {
            InitContext();
        }

        byte[] pcm = AudioLoader.Load(File.Open(filename, FileMode.Open), out ALFormat format, out int size, out int frequency);

        return new Audio(pcm, format, size, frequency);
    }

    /// <summary>
    /// Loads the mp3 file.
    /// </summary>
    /// <param name="filename">file name</param>
    /// <returns>loaded audio</returns>
    public static Audio LoadMp3( string filename )
    {
        NlcHelper.FileThrow(filename);

        if (Device == ALDevice.Null)
        {
            InitContext();
        }

        long size;
        byte[] pcm;
        int channels;
        int bps;
        int freq;

        using (Mp3FileReader reader = new Mp3FileReader(filename))
        {
            size = reader.Length;
            pcm = new byte[size];
            reader.Read(pcm, 0, (int)size);
            channels = reader.WaveFormat.Channels;
            bps = reader.WaveFormat.BitsPerSample;
            freq = reader.WaveFormat.SampleRate;
        }

        return new Audio(pcm, AudioLoader.GetSoundFormat(channels, bps), pcm.Length, freq);
    }
    
    /// <summary>
    /// Plays the audio.
    /// </summary>
    public void Play()
    {
        AL.Listener(ALListenerf.Gain, _volume);

        ALC.MakeContextCurrent(Context);
        AL.SourcePlay(_source);

        _playing = true;
    }

    /// <summary>
    /// Pauses the audio.
    /// </summary>
    public void Pause()
    {
        AL.SourcePause(_source);
        _playing = false;
    }

    /// <summary>
    /// Stops the audio.
    /// </summary>
    public void Stop()
    {
        AL.SourceStop(_source);
        _playing = false;
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            AL.DeleteSource(_source);
            AL.DeleteBuffer(_buffer);

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resource used by <see cref="Audio"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// A destructor.
    /// </summary>
    ~Audio()
    {
        Dispose(false);
    }
}




//
//  Source code from MonoGame
//
internal class AudioLoader
{
    private AudioLoader()
    {
    }

    public static ALFormat GetSoundFormat(int channels, int bits)
    {
        switch (channels)
        {
            case 1: return bits == 8 ? OpenTK.Audio.OpenAL.ALFormat.Mono8 : OpenTK.Audio.OpenAL.ALFormat.Mono16;
            case 2: return bits == 8 ? OpenTK.Audio.OpenAL.ALFormat.Stereo8 : OpenTK.Audio.OpenAL.ALFormat.Stereo16;
            default: throw new NotSupportedException("The specified sound format is not supported.");
        }
    }


    public static byte[] Load(Stream data, out ALFormat format, out int size, out int frequency)
    {
        byte[] audioData = null;
        format = ALFormat.Mono8;
        size = 0;
        frequency = 0;

        using (BinaryReader reader = new BinaryReader(data))
        {
            // decide which data type is this

            // for now we'll only support wave files
            audioData = LoadWave(reader, out format, out size, out frequency);
        }

        return audioData;
    }

    private static byte[] LoadWave(BinaryReader reader, out ALFormat format, out int size, out int frequency)
    {
        // code based on opentk exemple

        byte[] audioData;

        //header
        string signature = new string(reader.ReadChars(4));
        if (signature != "RIFF")
        {
            throw new NotSupportedException("Specified stream is not a wave file.");
        }

        reader.ReadInt32(); // riff_chunck_size

        string wformat = new string(reader.ReadChars(4));
        if (wformat != "WAVE")
        {
            throw new NotSupportedException("Specified stream is not a wave file.");
        }

        // WAVE header
        string format_signature = new string(reader.ReadChars(4));
        while (format_signature != "fmt ")
        {
            reader.ReadBytes(reader.ReadInt32());
            format_signature = new string(reader.ReadChars(4));
        }

        int format_chunk_size = reader.ReadInt32();

        // total bytes read: tbp
        int audio_format = reader.ReadInt16(); // 2
        int num_channels = reader.ReadInt16(); // 4
        int sample_rate = reader.ReadInt32();  // 8
        reader.ReadInt32();    // 12, byte_rate
        reader.ReadInt16();  // 14, block_align
        int bits_per_sample = reader.ReadInt16(); // 16

        if (audio_format != 1)
        {
            throw new NotSupportedException("Wave compression is not supported.");
        }

        // reads residual bytes
        if (format_chunk_size > 16)
            reader.ReadBytes(format_chunk_size - 16);

        string data_signature = new string(reader.ReadChars(4));

        while (data_signature.ToLowerInvariant() != "data")
        {
            reader.ReadBytes(reader.ReadInt32());
            data_signature = new string(reader.ReadChars(4));
        }

        if (data_signature != "data")
        {
            throw new NotSupportedException("Specified wave file is not supported.");
        }

        int data_chunk_size = reader.ReadInt32();

        frequency = sample_rate;
        format = GetSoundFormat(num_channels, bits_per_sample);
        audioData = reader.ReadBytes((int)reader.BaseStream.Length);
        size = data_chunk_size;

        return audioData;
    }
}