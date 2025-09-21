using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Whatwapp.Core.Audio;
using Whatwapp.Core.Extensions;
using Whatwapp.Core.Utils;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BlockGroupAnimationController
    {
        private readonly GameController _gameController;
        private readonly AnimationSettings _animationSettings;
        private readonly FoundationsController _foundationsController;
        private readonly BlockFactory _blockFactory;

        public BlockGroupAnimationController(GameController gameController, AnimationSettings animationSettings,
            FoundationsController foundationsController, BlockFactory blockFactory)
        {
            _gameController = gameController;
            _animationSettings = animationSettings;
            _foundationsController = foundationsController;
            _blockFactory = blockFactory;
        }
        
        public void PlayMoveBlocksAnimation(List<Cell> movingCells, Board board, Action onAnimationComplete)
        {
            var sequence = DOTween.Sequence();
            
            foreach (var cell in movingCells)
            {
                var block = cell.Block;
                var targetCell = board.GetCell(cell.Coordinates.x, cell.Coordinates.y + 1);
                targetCell.Block = block;
                cell.Block = null;
                sequence.Join(
                    DOTween.Sequence()
                        .AppendInterval(_animationSettings.BlockMoveDelay)
                        .Append(block.transform.DOMove(targetCell.Position, _animationSettings.BlockMoveDuration))
                        .OnComplete(() =>
                        {
                            block.Visual.ShakeScale();
                        }));
            }

            sequence.OnComplete(onAnimationComplete.Invoke);
            sequence.Play();
        }

        public void PlayMergeAnimation(List<List<Cell>> mergeableGroups, Action onAnimationComplete)
        {
            var sequence = DOTween.Sequence();
            foreach (var group in mergeableGroups)
            {
                var seedHash = new HashSet<BlockSeed>();
                var firstCell = group[0];
                var value = firstCell.Block.Value;
                var seed = firstCell.Block.Seed;
                seedHash.Add(seed);
                var groupSequence = DOTween.Sequence();
                var tremorSequence = DOTween.Sequence();
                foreach (var cell in group)
                {
                    seedHash.Add(cell.Block.Seed);
                    tremorSequence.Join(cell.Block.transform.DOShakeScale(_animationSettings.TremorDuration,
                        _animationSettings.TremorStrength));
                }

                Debug.Log("Seed hash count: " + seedHash.Count);

                groupSequence.Append(tremorSequence);
                for (var i = group.Count - 1; i > 0; i--)
                {
                    var cell = group[i];
                    var block = cell.Block;
                    var targetCell = group[i - 1];
                    var targetPos = targetCell.transform.position;
                    var blockSequence = DOTween.Sequence();
                    blockSequence.Append(block.transform.DOMove(targetPos, _animationSettings.MergeDuration));
                    blockSequence.Join(block.transform.DOScale(Vector3.zero, _animationSettings.MergeDuration).OnStart(
                        () => { SFXManager.Instance.PlayOneShot(Consts.SFX_PlayBlock); }));
                    blockSequence.SetDelay(_animationSettings.MergeDuration);
                    blockSequence.OnComplete(() =>
                    {
                        SFXManager.Instance.PlayOneShot(Consts.SFX_MergeBlocks);
                        targetCell.Block = null;
                        _gameController.Score += mergeableGroups.Count * group.Count;
                        block.Remove();
                    });
                    groupSequence.Join(blockSequence);
                }

                var finalSequence = DOTween.Sequence();
                finalSequence.Append(firstCell.Block.transform.DOScale(0, _animationSettings.MergeDuration)
                    .OnComplete(() =>
                    {
                        firstCell.Block.Remove();
                        firstCell.Block = null;
                    }));
                groupSequence.Join(finalSequence);
                groupSequence.OnComplete(() =>
                {
                    var nextValue = value.Next(true);
                    var randomSeed = EnumUtils.GetRandom<BlockSeed>();
                    var newBlock = _blockFactory.Create(nextValue, seed);
                    firstCell.Block = newBlock;
                    newBlock.transform.localScale = Vector3.zero;
                    newBlock.transform.DOScale(Vector3.one, _animationSettings.MergeDuration).SetEase(Ease.OutBack);

                    foreach (var seedInGroup in seedHash)
                    {
                        Debug.Log("Seed in group: " + seedInGroup);
                        var info = new BlockToFoundationInfo(seedInGroup, value, firstCell.Position);
                        if (_foundationsController.TryAndAttach(info))
                        {
                            SFXManager.Instance.PlayOneShot(Consts.GetFoundationSFX(seedInGroup));
                            _gameController.Score += Consts.FOUNDATION_POINTS;
                        }
                    }
                });

                sequence.Append(groupSequence);
            }

            sequence.OnComplete(onAnimationComplete.Invoke);
            sequence.Play();
        }
    }
}