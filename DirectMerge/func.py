from random import randrange as rand
import psutil
import math

def print_file(path):
    with open(path, "rb") as file:
        num = file.read(32)
        while num:
            num = int.from_bytes(num, 'big')
            print(num, end = ' ')
            num = file.read(32)
    return

def clear_file(path):
    with open(path, "wb") as file:
        file.write(b'')

def print_all():
    print_file("A.bin")
    print()
    print_file("B.bin")
    print()
    print_file("C.bin")
    print()
    return

def generate_file_a(n):
    fa = open("A.bin", 'wb')
    for i in range(n):
        fa.write(rand(0, 100).to_bytes(32,'big'))
    fa.close()
    return

def split_to_files(n, iteration):
    fa = open('A.bin', 'br')
    fb = open('B.bin', 'wb')
    fc = open('C.bin', 'wb')

    i = 0
    while(i < n):
        k1 = 0
        while(k1 != 2 ** iteration):
            fb.write(fa.read(32))
            i += 1
            k1 += 1

        k2 = 0
        while (k2 != 2 ** iteration):
            fc.write(fa.read(32))
            i += 1
            k2 += 1

    fa.close()
    fb.close()
    fc.close()
    return

def merge(n, iteration):
    fa = open('A.bin', 'wb')
    fb = open('B.bin', 'rb')
    fc = open('C.bin', 'rb')

    i = 0
    k1 = 1
    k2 = 1

    el1 = int.from_bytes(fb.read(32),'big')
    k1 += 1

    el2 = int.from_bytes(fc.read(32),'big')
    k2 += 1

    while (i < n):
        k1 = 1
        k2 = 1
        while (k1 != 2 ** iteration + 1 and k2 != 2 ** iteration + 1):
            if(el1 <= el2):
                fa.write(el1.to_bytes(32,'big'))
                el1 = int.from_bytes(fb.read(32),'big')
                k1 += 1
                i += 1
            else:
                fa.write(el2.to_bytes(32,'big'))
                el2 = int.from_bytes(fc.read(32),'big')
                k2 += 1
                i += 1

        while (k1 != 2 ** iteration + 1):
            fa.write(el1.to_bytes(32, 'big'))
            k1 += 1
            i += 1
            el1 = int.from_bytes(fb.read(32), 'big')

        while (k2 != 2 ** iteration + 1):
            fa.write(el2.to_bytes(32, 'big'))
            k2 += 1
            i += 1
            el2 = int.from_bytes(fc.read(32), 'big')

    fa.close()
    fb.close()
    fc.close()
    return

def is_sorted(path):
    with open(path, "rb") as file:
        prev = file.read(32)
        num = file.read(32)
        while num:
            if (int.from_bytes(prev, 'big') > int.from_bytes(num, 'big')):
                return False
            prev = num
            num = file.read(32)
    return True

# Functions for modified algorithm

def pre_sort(path):
    available_ints = int(psutil.virtual_memory()[1]/32)
    clear_file("preSortedFile.bin")

    with open(path, "rb") as file:
        int_list = []
        while True:
            num = file.read(32)
            if num == b'':
                break

            for i in range(available_ints):
                int_list.append(int.from_bytes(num, 'big'))
                num = file.read(32)
                if num == b'':
                    break

            int_list.sort()

            with open('preSortedFile.bin', "ab") as preFile:
                for i in int_list:
                    preFile.write(i.to_bytes(32, 'big'))

    return math.log2(available_ints)
