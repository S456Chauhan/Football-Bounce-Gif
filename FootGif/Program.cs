using ImageMagick;

            
            int totalFrames = 30;
            int yMovementPerFrame = 20; // faster vertical bounce
            int delayBetweenFrames = 30; // faster animation
            bool repeat = true;

            string imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "images");
            string outputDirectory = "OutputImages";  //for output gif saved 


          //Output directory if not exist
            Directory.CreateDirectory(outputDirectory);

            List<MagickImage> imageFrames = new List<MagickImage>();

            using (var backgroundImage = new MagickImage(Path.Combine(imagesDirectory, "Playground.png")))
            {             

                for (int frameIndex = 0; frameIndex < totalFrames; frameIndex++)
                {
                    using (var footballImage = new MagickImage(Path.Combine(imagesDirectory, "Football.png")))
                    {
                        int initialX = (backgroundImage.Width - footballImage.Width) / 2; // horizontal center football
                       int initialY = backgroundImage.Height - footballImage.Height * 4;

                        int yOffset = yMovementPerFrame * frameIndex;

                        
                        int newY = initialY + yOffset;
                        newY = Math.Max(newY, initialY); //football don't go outside the image

                        var finalImage = backgroundImage.Clone();
                        finalImage.Composite(footballImage, initialX, newY, CompositeOperator.Over);

                        imageFrames.Add((MagickImage)finalImage);
                    }
                }
            }

            using (var gifImages = new MagickImageCollection())
            {
                foreach (var frame in imageFrames)
                {
                    gifImages.Add(frame);
                    frame.AnimationDelay = delayBetweenFrames;
                }

                if (!repeat)
                {
                    gifImages[gifImages.Count - 1].AnimationIterations = 1;
                }

                string outputGifPath = Path.Combine(outputDirectory, "football_bounce.gif");
                gifImages.Write(outputGifPath, MagickFormat.Gif);
            }

            Console.WriteLine("Gif Created Successfully");
