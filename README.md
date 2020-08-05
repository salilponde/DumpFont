# DumpFont

DumpFont is a command-line program to read data from font files.

The project currently supports reading the below tables from TrueType font files.

* cmap
* glyf
* head
* hhea
* hmtx
* loca
* maxp
* OS/2
* post

# Usage

## List tables in font file

    dumpfont -f fontfilename.ttf

## Display data in table

    dumpfont -f fontfilename.ttf -t tablename

Example to display table *head*

    dumpfont -f fontfilename.ttf -t head

## Display encoding table

    dumpfont -f fontfilename.ttf -e index

Example:

    # Display the cmap table. Each table row will have an (Index) column
    dumpfont -f fontfilename.ttf -t cmap
    
    # Use desired (Index) from previous command in -e switch
    dumpfont -f fontfilename.ttf -e 2           

## Display glyph details

    dumpfont -f fontfilename.ttf -g charactercode

Example to display glyph details for character code 65 (A):

    dumpfont -f fontfilename.ttf -g 65
    
# License

The project uses the MIT license. Read [LICENSE.txt](https://github.com/salslab/DumpFont/blob/master/LICENSE.txt) for the details.
