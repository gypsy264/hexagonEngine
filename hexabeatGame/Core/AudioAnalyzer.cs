using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using NAudio.Wave;

namespace Project.Core
{
    public class AudioAnalyzer : IDisposable
    {
        private int _buffer;
    private int _source;
    private ALContext _context;

    public AudioAnalyzer(string filePath)
    {
        // Initialize the audio context
        _context = new ALContext();

        // Generate a buffer and a source
        _buffer = AL.GenBuffer();
        _source = AL.GenSource();

        // Load the MP3 file and fill the buffer
        using (Mp3FileReader mp3FileReader = new Mp3FileReader(filePath))
        {
            WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(mp3FileReader);
            int channels = waveStream.WaveFormat.Channels;
            int bitsPerSample = waveStream.WaveFormat.BitsPerSample;
            int sampleRate = waveStream.WaveFormat.SampleRate;

            byte[] soundData = new byte[waveStream.Length];
            waveStream.Read(soundData, 0, soundData.Length);

            // Bind the buffer data to the source
            IntPtr ptr = Marshal.AllocHGlobal(soundData.Length);
            Marshal.Copy(soundData, 0, ptr, soundData.Length);
            AL.BufferData(_buffer, GetSoundFormat(channels, bitsPerSample), ptr, soundData.Length, sampleRate);
            AL.Source(_source, ALSourcei.Buffer, _buffer);
        }
    }

        public void Play()
        {
            // Play the sound
            AL.SourcePlay(_source);
        }

        public void Stop()
        {
            // Stop the sound
            AL.SourceStop(_source);
        }

        private void LoadWave(string filename, out int channels, out int bits, out int rate, out byte[] soundData)
        {
            using (FileStream fs = File.OpenRead(filename))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();
                soundData = reader.ReadBytes(data_chunk_size);

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;
            }
        }

        private ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        public void Dispose()
        {
            AL.DeleteSource(_source);
            AL.DeleteBuffer(_buffer);
        }
    }
}