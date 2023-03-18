import PyInstaller.__main__, logging, argparse, platform, os

parser = argparse.ArgumentParser(description="Build configuration for Pyinstaller.")
parser.add_argument("--icon", help="path to the icon file")
parser.add_argument("--spec", help="path to your spec file")
parser.add_argument("--name", help="path to your script")
parser.add_argument("--out", help="output path of the executable")
args = parser.parse_args()

if args.spec and os.path.isfile(args.spec):
    abs_path = os.path.abspath(__file__)
    with open(args.spec, "r") as file:
        spec = [i.strip() for i in file.readlines()]
        for i in spec:
            os.system(f"python \"{abs_path}\" {i}")
            print()

else:
    # Set PyInstaller log level to WARNING to hide the info messages
    logger = logging.getLogger('PyInstaller')

    logger.setLevel(logging.INFO)

    logger.info('Cxthon')
    logger.info('Python: %s', platform.python_version())
    logger.info('Platform: %s' % platform.platform())
    logger.info('Build configurations.')
    logger.info('name: %s', args.name)
    logger.info('out: %s', args.out)
    logger.info('icon: %s', args.icon) if args.icon else None
    logger.info('Building.')

    logger.setLevel(logging.WARNING)

    options = [
        args.name,
        "--distpath", args.out,
        "--icon", args.icon,
        "--clean",
        "--onefile"
    ]

    PyInstaller.__main__.run(options)
