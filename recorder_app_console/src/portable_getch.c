#ifdef WIN32
    #include <conio.h>
#endif

#ifdef __linux__
    // https://gist.github.com/ninovsnino/f910701ea912f03b73a1d0895d213b0e
    #include <termios.h>
    #include <stdio.h>

    static struct termios old, new;
    void init()
    {
        tcgetattr(0, &old);
        new = old;
        new.c_lflag &= ~ICANON;
        new.c_lflag &= ~ECHO;
        tcsetattr(0, TCSANOW, &new);
    }
    void reset()
    {
        tcsetattr(0, TCSANOW, &old);
    }
    int getch_linux()
    {
        init();
        int ch = getchar();
        reset();
        return ch;
    }
#endif

int portable_getch()
{
    #if defined(WIN32)
        return _getch();
    #elif defined(__linux__)
        return getch_linux();
    #endif
    return -1;
}