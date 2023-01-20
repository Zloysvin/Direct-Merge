from func import *
import time
from shutil import copyfile as copyf

n = 4194304
i = 0

# Algorithm

def unmodified_alg(n, i):
    generate_file_a(n)
    t1 = time.time()

    while (2 ** (i + 1) <= n):
        split_to_files(n, i)
        merge(n, i)
        i += 1

    t2 = time.time()

    if (is_sorted('A.bin')):
        print("\nSORTED")
    else:
        print("\nNOT SORTED")

    print("Used time: " + str(t2 - t1) + "s")
    return

def modified_alg(n, i):
    generate_file_a(n)
    t1 = time.time()
    i = pre_sort('A.bin')
    copyf('preSortedFile.bin', 'A.bin')

    while(2 ** (i + 1) <= n):
        split_to_files(n, i)
        merge(n, i)
        i += 1

    t2 = time.time()

    if(is_sorted('A.bin')):
        print("\nSORTED")
    else:
        print("\nNOT SORTED")

    print("Used time: " + str(t2 - t1) + "s")
    return

unmodified_alg(n, i)
modified_alg(n, i)