cmake_minimum_required(VERSION 3.15)

project(recorder-lib LANGUAGES CXX)

# Make CMake happy by adding at least one file
add_library(recorder-lib STATIC src/dummy.cpp)
target_compile_features(recorder-lib PRIVATE cxx_std_20)

if (WIN32)
    find_package(gameinput CONFIG REQUIRED)
    find_package(wil CONFIG REQUIRED)
    find_package(boost_container CONFIG REQUIRED)

    target_link_libraries(recorder-lib PRIVATE Microsoft::GameInput)
    target_link_libraries(recorder-lib PRIVATE WIL::WIL)
    target_link_libraries(recorder-lib PRIVATE Boost::container)

    target_sources(recorder-lib PRIVATE src/recorder_win_gameinput.cpp)
else()
    message(FATAL_ERROR "A recorder hasn't been implemented for this platform")
endif()

target_include_directories(recorder-lib PUBLIC include)
