%%-*- LaTeX -*-%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%
% qresume.cls
%
% Class file for résumés and vitæ.
%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Title:        cjw-resume.cls
% Author:       Colin J. Wynne <cwynne@mts.jhu.edu>
% Modifiied by: Joaquim R. R. A. Martins
% Date:         Tue Sep  9 11:41:20 1997
% Last Rev.:    Tue Feb  6 18:20:14 2001
%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

% Identify the class.
\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{qcv}%

% Default option---pass to article.cls, since that is the base.
\DeclareOption*{\PassOptionsToClass{\CurrentOption}{article}}

% Do options.
\ProcessOptions

% Load the base class and required packages.
\LoadClass{article}
%  \RequirePackage{cjwmacro}
%  \RequirePackage{cjwoutl}
  \RequirePackage{path}

% Error information should be more than the default.
\setcounter{errorcontextlines}{5}

%%%%%%%%%% Pagestyle %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
\newcommand{\ps@resume}{%
  \let\@mkboth\@gobbletwo
  \def\@oddhead{\pagenofont{\footnotesize%
    \itshape\@name}\hfil\thepage}%
  \let\@evenhead\@oddhead
  \let\@oddfoot\@empty
  \let\@evenfoot\@empty}

\pagestyle{resume}
\thispagestyle{empty}

%%%%%%%%%% Title %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
\newtoks    \@NameToks
%\newtoks    \@FirstToks
%\newtoks    \@MiToks
%\newtoks    \@LastToks
\newtoks    \@AddressToks
\newtoks    \@UrlToks
\newtoks    \@EMailToks

%\def\name#1{{\Large\centering#1\\[2\baselineskip]}}
%\def\name#1#2#3{\global\@NameToks={#1 #2~#3}
%        \global\@FirstToks={#1}\global\@MiToks{#2}\global\@LastToks{#3}}

\def\name#1{\global\@NameToks={#1}}

\def\address #1{\global\@AddressToks={#1}}
\def\url     #1{\global\@UrlToks={#1}}
\def\email   #1{\global\@EMailToks={#1}}
  \name{}   \address{}
  \url{}    \email{}

\def\BigSize{\Large}

%\def\@capfirst #1#2!!!{{\BigSize \uppercase{#1}}\uppercase{#2}}

\def\maketitle{
  \edef\@name{\the\@NameToks}
  \edef\@addr{\the\@AddressToks}
  \edef\@url {\the\@UrlToks}
  \edef\@mail{\the\@EMailToks}
    %
  \ifx\@empty\@addr
  \else
    \@AddressToks=\expandafter{\the\@AddressToks\\}
  \fi
    %
  \ifx\@empty\@url
  \else
    \@UrlToks=\expandafter{\expandafter\path\expandafter+\the\@UrlToks+\\}
  \fi
    %
  \ifx\@empty\@email
  \else
    \@EMailToks=\expandafter{\expandafter\path\expandafter+\the\@EMailToks+}
  \fi
    %
  \gdef\@name{\the\@NameToks}
    %
  \begin{center}
    {\bfseries\Large%
    \expandafter\the\@NameToks} \\ [6pt]
%    \expandafter\@capfirst\the\@MiToks!!!   
%    \expandafter\@capfirst\the\@LastToks!!!}    \\ %[-6pt]
%    \hspace*{1in}\hrulefill\hspace*{1in}        \\
    {\small\strut%
    \the\@AddressToks%
    \the\@UrlToks%
    \the\@EMailToks}
  \end{center}

  \setlength{\leftskip}{0.5in}
  \setlength{\parindent}{0pt}
  \renewcommand{\section}[1]{%
    \@oldparskip=\parskip\relax
    \medskip\hskip -0.5in{\large\textsc{##1}}\\}
}

  %%%%%%%%%% Entries %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
\newskip    \@oldparskip

%\newenvironment{entry}[1]%
%  {\begingroup\parskip=0pt\textbf{#1}}
%  {\kern0pt\relax\endgroup\vskip\@oldparskip}
%\newenvironment{altentry}[1]%
%  {\begingroup\parskip=0pt\textsc{#1}}
%  {\kern0pt\relax\endgroup\vskip\@oldparskip}

\newcommand{\RefPhrase} {References available upon request.}
\newcommand{\references}{\bigskip\bigskip
    \hskip -0.5in\emph{\RefPhrase}}

% Follwoing is taken from resume.sty

% use this to print your name at the top of the page
%\def\name#1{{\Large\centering#1\\[2\baselineskip]}}

% one address
%\def\address#1{\hbox to \hsize{\hfil\@tablebox{#1}\hfil}}

% two addresses (say, home and work).  They get printed side by side
%\def\addresses#1#2{\hbox to \hsize{\@tablebox{#1}\hfil\@tablebox{#2}}}

%\def\@tablebox#1{\begin{tabular}[t]{@{}l@{}}#1\end{tabular}}

\newbox\@title

% Most of the resume goes inside a llist environment.  Within it, use
% the \sectiontitle{title} command to begin new sections.  The title
% of the section is put in the left margin.  The section text begins
% on the next line if the sectiontitle is too long.
\newenvironment{llist}{\begin{list}{}{\setlength
% \labelwidth{0in}\setlength\leftmargin{\labelwidth}\addtolength
% \leftmargin{\labelsep}\itemsep 10pt plus 2pt minus 2pt
% \parsep 4pt plus 2pt minus 2pt
 \labelwidth{0.2in}\setlength\leftmargin{\labelwidth}\addtolength
 \leftmargin{\labelsep}\itemsep 10pt
 \parsep 6pt
% \def\sectiontitle##1{\item[{\bf##1}]\setbox\@title=\hbox{\bf##1}\ifdim
%   \wd\@title>\labelwidth \leavevmode \\ \else \fi}
 \def\sectiontitle##1{\item[{\large\bf##1}]\setbox\@title=\hbox{\bf##1}\leavevmode
 \vspace{\parsep} \\}
 \def\makelabel##1{##1}}}{\end{list}}

% like itemize but bullets are at the left margin
\newenvironment{items}{\begin{list}{$\bullet$}
  {\setlength\labelwidth{.5em}\setlength\leftmargin{1em}
  \def\makelabel##1{##1\hfill}}}{\end{list}}

% Use the \employer and \location commands to print the names and
% cities of your employers.
\def\employer#1{{\bf #1}}
% The location is always flush right.  It is moved to the next
% line if there is not room left on this one.  
% See the TeXbook Chapter 14.
\def\location#1{{\hfill\penalty50\hskip2em
  \hbox{}\nobreak\hfill #1\finalhyphendemerits=0 \\}}
%\def\location#1{{\unskip\nobreak\hfill\penalty50\hskip2em
%  \hbox{}\nobreak\hfill #1\finalhyphendemerits=0 \\}}

% This prints the date flush left.
% The major shortcoming of this file:
% There needs to be an option to print the date in the left margin,
% because many people like to do it that way.
%\def\job#1{{\em #1}\\[2pt]}
%\def\dates#1{{\em #1}\\[2pt]}
%\def\jobdates#1#2{ {\em #1} \hfill\penalty50\hskip2em
%  \hbox{}\nobreak\hfill {\em #2} \finalhyphendemerits=0 \\[2pt] }
\def\job#1{{\em #1}\\}
\def\dates#1{{\em #1}\\}
% \jobdates takes 2 arguments, the job title and the dates
\def\jobdates#1#2{ {\em #1} \hfill\penalty50\hskip2em
  \hbox{}\nobreak\hfill {\em #2} \finalhyphendemerits=0 \\}

\endinput
