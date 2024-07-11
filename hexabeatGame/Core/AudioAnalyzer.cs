using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using OpenTK.Audio.OpenAL;

public class AudioAnalyzer
{
    private int _buffer;
    private int _source;
    private byte[] _audioData;
    private GCHandle _audioDataHandle;
    private Thread _analyzeThread;
    private bool _isPlaying = false;
    private int _sampleRate;
    private int _channels;
    private readonly float[] _fftBuffer;
    public event Action OnBeatDetected;

    public AudioAnalyzer(string filePath)
    {
        // Load audio file data
        _audioData = LoadWavFile(filePath, out _sampleRate, out _channels);
        _fftBuffer = new float[1024];

        // Pin audio data
        _audioDataHandle = GCHandle.Alloc(_audioData, GCHandleType.Pinned);

        // Initialize OpenAL
        _buffer = AL.GenBuffer();
        CheckALError("AL.GenBuffer");

        _source = AL.GenSource();
        CheckALError("AL.GenSource");

        ALFormat format = _channels == 1 ? ALFormat.Mono16 : ALFormat.Stereo16;
        AL.BufferData(_buffer, format, _audioDataHandle.AddrOfPinnedObject(), _audioData.Length, _sampleRate);
        CheckALError("AL.BufferData");

        AL.Source(_source, ALSourcei.Buffer, _buffer);
        CheckALError("AL.Source (Buffer)");
        
        AL.Source(_source, ALSourcef.Gain, 1.0f); // Set volume
        CheckALError("AL.Source (Gain)");

        AL.Source(_source, ALSource3f.Position, 0.0f, 0.0f, 0.0f); // Set source position
        CheckALError("AL.Source (Position)");

        AL.Listener(ALListener3f.Position, 0.0f, 0.0f, 0.0f); // Set listener position
        CheckALError("AL.Listener (Position)");
    }

    public void Play()
    {
        _isPlaying = true;
        AL.SourcePlay(_source);
        CheckALError("AL.SourcePlay");

        AL.GetSource(_source, ALGetSourcei.SourceState, out int state);
        Console.WriteLine($"Source state after play: {(ALSourceState)state}");
        
        _analyzeThread = new Thread(AnalyzeAudio);
        _analyzeThread.Start();
    }

    public void Stop()
    {
        _isPlaying = false;
        AL.SourceStop(_source);
        CheckALError("AL.SourceStop");

        _analyzeThread?.Join();
    }

    private void AnalyzeAudio()
    {
        while (_isPlaying)
        {
            // Placeholder beat detection logic
            for (int i = 0; i < _fftBuffer.Length; i++)
            {
                if (_fftBuffer[i] > 0.5f) // Adjust threshold as necessary
                {
                    OnBeatDetected?.Invoke();
                }
            }

            Thread.Sleep(100); // Adjust as needed
        }
    }

    private byte[] LoadWavFile(string filePath, out int sampleRate, out int channels)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            // Read the RIFF header
            string chunkID = new string(reader.ReadChars(4));
            if (chunkID != "RIFF")
                throw new NotSupportedException("Invalid WAV file: Missing RIFF header.");

            int fileSize = reader.ReadInt32();
            string format = new string(reader.ReadChars(4));
            if (format != "WAVE")
                throw new NotSupportedException("Invalid WAV file: Missing WAVE header.");

            // Read the fmt chunk
            string fmtChunkID = new string(reader.ReadChars(4));
            if (fmtChunkID != "fmt ")
                throw new NotSupportedException("Invalid WAV file: Missing fmt chunk.");

            int fmtChunkSize = reader.ReadInt32();
            int audioFormat = reader.ReadInt16();
            if (audioFormat != 1)
                throw new NotSupportedException("Invalid WAV file: Only PCM encoding is supported.");

            channels = reader.ReadInt16();
            sampleRate = reader.ReadInt32();
            int byteRate = reader.ReadInt32();
            int blockAlign = reader.ReadInt16();
            short bitsPerSample = reader.ReadInt16();

            if (bitsPerSample != 8 && bitsPerSample != 16 && bitsPerSample != 24)
                throw new NotSupportedException("Only 8-bit, 16-bit, and 24-bit WAV files are supported.");

            // Read the data chunk
            string dataChunkID = new string(reader.ReadChars(4));
            if (dataChunkID != "data")
                throw new NotSupportedException("Invalid WAV file: Missing data chunk.");

            int dataSize = reader.ReadInt32();
            byte[] data = reader.ReadBytes(dataSize);

            Console.WriteLine($"Channels: {channels}, SampleRate: {sampleRate}, BitsPerSample: {bitsPerSample}, DataSize: {dataSize}");

            return data;
        }
    }

    private void CheckALError(string operation)
    {
        ALError error = AL.GetError();
        if (error != ALError.NoError)
        {
            throw new InvalidOperationException($"OpenAL error {error} during {operation}");
        }
    }

    ~AudioAnalyzer()
    {
        if (_audioDataHandle.IsAllocated)
        {
            _audioDataHandle.Free();
        }

        AL.DeleteSource(_source);
        AL.DeleteBuffer(_buffer);
    }
}