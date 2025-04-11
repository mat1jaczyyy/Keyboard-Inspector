#include <array>
#include <linux/input-event-codes.h>

// Event code for keyboard keys, https://en.wikipedia.org/wiki/Keyboard_layout
unsigned short code_keyboard[] = {
    // Function keys row
    KEY_ESC, KEY_F1, KEY_F2, KEY_F3, KEY_F4, KEY_F5, KEY_F6, KEY_F7, KEY_F8,
    KEY_F9, KEY_F10, KEY_F11, KEY_F12,

    // Number row
    KEY_GRAVE, KEY_1, KEY_2, KEY_3, KEY_4, KEY_5, KEY_6, KEY_7, KEY_8, KEY_9,
    KEY_0, KEY_MINUS, KEY_EQUAL, KEY_BACKSPACE,

    // Top row
    KEY_TAB, KEY_Q, KEY_W, KEY_E, KEY_R, KEY_T, KEY_Y, KEY_U, KEY_I, KEY_O,
    KEY_P, KEY_LEFTBRACE, KEY_RIGHTBRACE, KEY_BACKSLASH,

    // Home row
    KEY_CAPSLOCK, KEY_A, KEY_S, KEY_D, KEY_F, KEY_G, KEY_H, KEY_J, KEY_K, KEY_L,
    KEY_SEMICOLON, KEY_APOSTROPHE, KEY_ENTER,

    // Bottom row
    KEY_LEFTSHIFT, KEY_Z, KEY_X, KEY_C, KEY_V, KEY_B, KEY_N, KEY_M, KEY_COMMA,
    KEY_DOT, KEY_SLASH, KEY_RIGHTSHIFT, KEY_SPACE,

    // Modifier keys
    KEY_LEFTCTRL, KEY_LEFTMETA, KEY_LEFTALT, KEY_RIGHTALT, KEY_MENU,
    KEY_RIGHTMETA, KEY_RIGHTCTRL,

    // Arrow key clusters
    KEY_SYSRQ, KEY_SCROLLLOCK, KEY_BREAK, KEY_INSERT, KEY_HOME, KEY_PAGEUP,
    KEY_DELETE, KEY_END, KEY_PAGEDOWN, KEY_UP, KEY_LEFT, KEY_DOWN, KEY_RIGHT,

    // Number pad
    KEY_NUMLOCK, KEY_KPSLASH, KEY_KPASTERISK, KEY_KPMINUS, KEY_KPPLUS,
    KEY_KPENTER, KEY_KP0, KEY_KP1, KEY_KP2, KEY_KP3, KEY_KP4, KEY_KP5, KEY_KP6,
    KEY_KP7, KEY_KP8, KEY_KP9, KEY_KPDOT
};

// Event code for mouse buttons
unsigned short code_mouse[] = { BTN_LEFT, BTN_RIGHT, BTN_MIDDLE };
