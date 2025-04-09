cmake_minimum_required(VERSION 3.15)

project(recorder-app-console LANGUAGES CXX)

add_executable(recorder-app-console
    src/main.cpp
    src/portable_getch.c
)
target_compile_features(recorder-app-console PRIVATE cxx_std_20)
target_link_libraries(recorder-app-console PRIVATE recorder-lib)
