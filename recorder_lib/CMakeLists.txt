cmake_minimum_required(VERSION 3.15)

project(recorder-lib LANGUAGES CXX)

# Make CMake happy by adding at least one file
add_library(recorder-lib STATIC src/dummy.cpp)
target_compile_features(recorder-lib PRIVATE cxx_std_20)

find_package(boost_container CONFIG REQUIRED)
find_package(boost_unordered CONFIG REQUIRED)
find_package(boost_iterator CONFIG REQUIRED)
target_link_libraries(recorder-lib PRIVATE Boost::container Boost::unordered Boost::iterator)

if (WIN32)
    find_package(gameinput CONFIG REQUIRED)
    find_package(wil CONFIG REQUIRED)
    
    target_link_libraries(recorder-lib PRIVATE Microsoft::GameInput)
    target_link_libraries(recorder-lib PRIVATE WIL::WIL)

    target_sources(recorder-lib PRIVATE src/win/recorder_win_gameinput.cpp)
elseif(LINUX)
    find_package(PkgConfig)
    pkg_check_modules(LIBEVDEV REQUIRED libevdev)
    if (NOT LIBEVDEV_FOUND)
        message(FATAL_ERROR "Cannot find libevdev")
    endif()
    target_link_libraries(recorder-lib PRIVATE ${LIBEVDEV_LINK_LIBRARIES})
    target_include_directories(recorder-lib PRIVATE ${LIBEVDEV_INCLUDE_DIRS})

    target_sources(recorder-lib PRIVATE
        src/linux/evdev_key_codes.h
        src/linux/recorder_linux_libevdev.cpp
    )
else()
    message(FATAL_ERROR "A recorder hasn't been implemented for this platform")
endif()

target_include_directories(recorder-lib PUBLIC include)
