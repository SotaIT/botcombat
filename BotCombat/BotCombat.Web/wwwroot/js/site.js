function Game(model, canvasId, logId) {

    // init variables
    const map = model.Game.Map;
    const steps = model.Game.Steps;
    const scale = model.Scale;
    const width = map.Width * scale;
    const height = map.Height * scale;
    const bulletSpeed = model.BulletSpeed;
    const frameCount = 8;

    const canvas = $(canvasId);
    const log = $(logId)[0];

    canvas.attr('width', width);
    canvas.attr('height', height);

    const speed = 1 / 2 / 2 / 2;
    const frameInterval = speed * scale / frameCount;
    const objectSize = scale;
    let stepNumber = 0;
    let frameNumber = 0;

    const images = {};
    let loadingImageCount = 0;


    // functions

    function preLoadImages() {
        for (let im in model.Images)
            if (model.Images.hasOwnProperty(im)) {
                let img = new Image();
                img.src = model.Images[im];
                img.onload = imageLoaded();
                images[im] = img;
                loadingImageCount++;
            }
    }

    function imageLoaded() {
        loadingImageCount--;
    }

    function getBotImage(id) {
        return images[model.BotImages[id]];
    }

    function getBulletImage(id) {
        return images[model.BulletImages[id]];
    }

    function getShotImage(id) {
        return images[model.ShotImages[id]];
    }

    function getExplosionImage(id) {
        return images[model.ExplosionImages[id]];
    }

    function getBonusImage(id) {
        return images[model.BonusImages[id]];
    }

    function getTrapImage(id) {
        return images[model.TrapImages[id]];
    }

    function getWallImage(id) {
        return images[model.WallImages[id]];
    }

    function getBgImage() {
        return images[model.Background];
    }

    function computeSx() {
        let f = Math.floor(frameNumber * speed);
        while (f > frameCount - 1)
            f -= frameCount;
        return objectSize * f + objectSize;
    }

    function getPrevBot(id) {
        if (stepNumber > 0)
            for (let j = 0; j < steps[stepNumber - 1].Bt.length; j++) {
                const prev = steps[stepNumber - 1].Bt[j];
                if (prev.Id === id) {
                    return prev;
                }
            }
        return null;
    }

    function getDirOffset(dr) {
        switch (dr) {
            // up
            case 2:
                return 0;
            // right
            case 3:
                return objectSize;
            // down
            case 4:
                return objectSize * 2;
            // left
            case 5:
                return objectSize * 3;
            // none
            default:
                return -1;
        }
    }

    function getBotMoveOffset(x1, x2) {
        return x1 * scale + (x2 - x1) * frameNumber;
    }

    function drawBots(ctx, step) {

        for (let i = 0; i < step.Bt.length; i++) {
            const bt = step.Bt[i];
            let x = bt.X * scale, y = bt.Y * scale;
            let sx = 0, sy = 0;
            const prevBot = getPrevBot(bt.Id);
            if (prevBot) {
                sx = computeSx();
                sy = getDirOffset(bt.Dr);
                if (bt.X === prevBot.X && bt.Y === prevBot.Y) 
                    sx = 0;

                x = getBotMoveOffset(prevBot.X, bt.X);
                y = getBotMoveOffset(prevBot.Y, bt.Y);
            }

            ctx.drawImage(getBotImage(bt.Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
        }
    }

    function getPrevBullet(id, n) {
        if (stepNumber > 0)
            for (let j = 0; j < steps[stepNumber - 1].Bl.length; j++) {
                const prev = steps[stepNumber - 1].Bl[j];
                if (prev.Id === id && prev.N === n) {
                    return prev;
                }
            }
        return null;
    }

    function getBulletMoveOffset(prev, real, next) {
        if (next === prev) return prev * scale;

        const up = next > real;
        const down = next < real;
        let diff = real - prev;

        if (up || down) {

            const realScaled = real * scale;
            if (up)
                diff++;
            else
                diff--;
            const newReal = prev * scale + diff * frameNumber;
            return newReal > realScaled && up || newReal < realScaled && !up
                ? -1
                : newReal;
        }
        return prev * scale + diff * frameNumber;
    }

    function getNewPos(dr, x, y, itemSpeed) {
        switch (dr) {
            // up
            case 2:
                return { X: x, Y: y - itemSpeed };
            // right
            case 3:
                return { X: x + itemSpeed, Y: y };
            // down
            case 4:
                return { X: x, Y: y + itemSpeed };
            // left
            case 5:
                return { X: x - itemSpeed, Y: y };
            // none
            default:
                return { X: x, Y: y };
        }
    }

    function drawBullets(ctx, step) {

        for (let i = 0; i < step.Bl.length; i++) {
            const bl = step.Bl[i];

            let x = bl.X * scale, y = bl.Y * scale;
            let sx = 0, sy = 0;
            let show = true;

            let prev = getPrevBullet(bl.Id, bl.N);
            if (prev === null)
                prev = getPrevBot(bl.Id);
            if (prev) {
                sy = getDirOffset(bl.Dr);
                sx = computeSx();
                const next = getNewPos(bl.Dr, prev.X, prev.Y, bulletSpeed);
                x = getBulletMoveOffset(prev.X, bl.X, next.X);
                y = getBulletMoveOffset(prev.Y, bl.Y, next.Y);
                show = x !== -1 && y !== -1;
            }

            if(show)
                ctx.drawImage(getBulletImage(bl.Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
        }
    }

    function drawShots(ctx, step) {

        for (let i = 0; i < step.Ss.length; i++) {
            const s = step.Ss[i];
            const next = getNewPos(s.Dr, s.X, s.Y, 1);
            const x = next.X * scale,
                y = next.Y * scale,
                sy = getDirOffset(s.Dr),
                sx = computeSx();
            ctx.drawImage(getShotImage(s.Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
        }
    }

    function drawExplosions(ctx, step) {

        for (let i = 0; i < step.Es.length; i++) {
            const e = step.Es[i];
            const x = e.X * scale,
                y = e.Y * scale,
                sy = 0,
                sx = computeSx();
            ctx.drawImage(getExplosionImage(e.Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
        }
    }

    function drawBonuses(ctx, step) {
        for (let i = 0; i < step.Bs.length; i++) {
            const b = step.Bs[i];
            ctx.drawImage(getBonusImage(b.Id), b.X * scale, b.Y * scale, scale, scale);
        }
    }

    function drawTraps(ctx) {
        for (let i = 0; i < map.Traps.length; i++) {
            const t = map.Traps[i];
            ctx.drawImage(getTrapImage(t.Id), t.X * scale, t.Y * scale, scale, scale);
        }
    }

    function drawWalls(ctx) {
        for (let i = 0; i < map.Walls.length; i++) {
            const w = map.Walls[i];
            ctx.drawImage(getWallImage(w.Id), w.X * scale, w.Y * scale, scale, scale);
        }
    }

    function drawBackground(ctx) {
        let bg = getBgImage();
        ctx.drawImage(bg, 0, 0, width, height);
    }

    function startAnimation() {
        window.requestAnimationFrame(draw);
    }

    function draw() {

        if (steps.length === 0) {
            window.requestAnimationFrame(draw);
            return;
        }

        const step = steps[stepNumber];

        const ctx = canvas[0].getContext('2d');
        ctx.globalCompositeOperation = 'destination-over';
        ctx.clearRect(0, 0, width, height);

        drawShots(ctx, step);
        drawExplosions(ctx, step);
        drawWalls(ctx);
        drawBots(ctx, step);
        drawBullets(ctx, step);
        drawBonuses(ctx, step);
        drawTraps(ctx);
        drawBackground(ctx);

        frameNumber += frameInterval;
        if (frameNumber >= scale) {
            writeLogs(step);
            stepNumber++;
            frameNumber = 0;
        }

        if (stepNumber >= steps.length) {
            if (step.bots.length > 0)
                writeLog('Step ' + (stepNumber - 1) + ': Bot #' + step.bots[0].id + ' won!');
            writeLog('Game Over!');
            return;
        }

        window.requestAnimationFrame(draw);
    }

    function writeLogs(step) {
        for (let i = 0; i < step.L.length; i++) {
            const l = step.L[i];
            let msg = '';

            switch (l.T) {
                case 1:
                    msg = 'Bot #' + l.Si + ' made ' + l.V + ' damage to bot #' + l.Ti + ' at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 2:
                    msg = 'Bot #' + l.Ti + ' got ' + l.V + ' power from bonus at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 3:
                    msg = 'Bot #' + l.Ti + ' took ' + l.V + ' damage from trap at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 4:
                    msg = 'Bot #' + l.Ti + ' was killed at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 5:
                    msg = 'Bot #' + l.Si + ' made ' + l.V + ' ranged damage to bot #' + l.Ti + ' at (' + l.X + ', ' + l.Y + ')';
                    break;
            }

            writeLog('Step ' + stepNumber + ': ' + msg);
        }
    }

    function writeLog(s) {
        let record = document.createElement('div');
        record.innerHTML = s;
        log.insertBefore(record, log.firstChild);
    }

    // public functions
    this.play = function () {
        while (loadingImageCount > 0);

        startAnimation();
    };

    // init
    function init() {
        preLoadImages();
    }

    init();

}