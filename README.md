
# DumpFont

DumpFont is a command-line program to read data from font files.

The project currently supports reading the below tables from TrueType font files.

* cmap
* head
* hhea
* hmtx
* maxp
* OS/2
* post

# Usage

## List tables in font file

    dumpfont -f fontfilename.ttf

## Display data in table

    dumpfont -t tablename -f fontfilename.ttf

Example to display table *head*

    dumpfont -t head -f fontfilename.ttf
    
# License

The project uses the MIT license. Read LICENSE.txt for the details.