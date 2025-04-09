cmake_minimum_required(VERSION 3.15)
set(CMAKE_TOOLCHAIN_FILE "$ENV{VCPKG_ROOT}/scripts/buildsystems/vcpkg.cmake")

project(keyboard-inspector)

add_subdirectory(recorder_lib)
add_subdirectory(recorder_app_console)
