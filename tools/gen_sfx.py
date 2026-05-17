import math
import os
import random
import struct
import wave

RATE = 44100
OUT_DIR = os.path.join(os.path.dirname(__file__), "..", "Assets", "Audio")


def write_wav(name, samples):
    os.makedirs(OUT_DIR, exist_ok=True)
    path = os.path.join(OUT_DIR, name)
    with wave.open(path, "w") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(RATE)
        frames = bytearray()
        for s in samples:
            v = int(max(-1.0, min(1.0, s)) * 32767)
            frames += struct.pack("<h", v)
        w.writeframes(bytes(frames))


def enemy_died():
    dur = 0.12
    n = int(RATE * dur)
    out = []
    for i in range(n):
        t = i / RATE
        freq = 820 - (620 * (i / n))
        env = math.exp(-22 * t)
        out.append(0.6 * math.sin(2 * math.pi * freq * t) * env)
    return out


def core_damaged():
    dur = 0.20
    n = int(RATE * dur)
    out = []
    for i in range(n):
        t = i / RATE
        env = math.exp(-16 * t)
        tone = math.sin(2 * math.pi * 130 * t)
        noise = random.uniform(-1, 1) * 0.3
        out.append(0.7 * (tone + noise) * env)
    return out


def core_destroyed():
    dur = 0.85
    n = int(RATE * dur)
    out = []
    for i in range(n):
        t = i / RATE
        env = math.exp(-4.5 * t)
        rumble = math.sin(2 * math.pi * 55 * t)
        noise = random.uniform(-1, 1)
        out.append(0.8 * (0.5 * rumble + 0.5 * noise) * env)
    return out


if __name__ == "__main__":
    write_wav("enemy_died.wav", enemy_died())
    write_wav("core_damaged.wav", core_damaged())
    write_wav("core_destroyed.wav", core_destroyed())
    print("SFX written to", os.path.abspath(OUT_DIR))
