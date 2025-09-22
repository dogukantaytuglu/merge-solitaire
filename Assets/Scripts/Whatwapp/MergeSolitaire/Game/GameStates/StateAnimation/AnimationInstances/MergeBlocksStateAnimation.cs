using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Whatwapp.Core.Audio;
using Whatwapp.Core.Extensions;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MergeBlocksStateAnimation : IStateAnimation
    {
        public bool IsAnimationActive { get; private set; }

        private readonly GameController _gameController;
        private readonly FoundationsController _foundationsController;
        private readonly BlockFactory _blockFactory;

        private Sequence _sequence;
        public MergeBlocksStateAnimation(GameController gameController, FoundationsController foundationsController, BlockFactory blockFactory)
        {
            _gameController = gameController;
            _foundationsController = foundationsController;
            _blockFactory = blockFactory;
        }
        
        public void Play()
        {
            IsAnimationActive = true;
             _sequence = DOTween.Sequence();
             var mergeableGroups = MergeBlocksState.MergeableGroupsBuffer;
             
            foreach (var group in mergeableGroups)
            {
                var seedHash = new HashSet<BlockSeed>();
                var firstCell = group[0];
                if (firstCell.Block is not MergeBlock firstBlock) continue;
                var value = firstBlock.Value;
                var seed = firstBlock.Seed;
                seedHash.Add(seed);
                var groupSequence = DOTween.Sequence();
                var tremorSequence = DOTween.Sequence();
                foreach (var cell in group)
                {
                    if (cell.Block is not MergeBlock mergeBlock) continue;
                    seedHash.Add(mergeBlock.Seed);
                    tremorSequence.Join(mergeBlock.PlayTremorAnimation());
                }
            
                Debug.Log("Seed hash count: " + seedHash.Count);
            
                groupSequence.Append(tremorSequence);
                for (var i = group.Count - 1; i > 0; i--)
                {
                    var cell = group[i];
                    if (cell.Block is not MergeBlock mergeBlock) continue;
                    var block = cell.Block;
                    var targetCell = group[i - 1];
                    var targetPos = targetCell.transform.position;
                    var blockSequence = mergeBlock.PlayMergeAnimation(targetPos);
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
                finalSequence.Append(firstBlock.PlayScaleDownAnimation())
                    .OnComplete(() =>
                    {
                        firstBlock.Remove();
                        firstBlock = null;
                    });
                groupSequence.Join(finalSequence);
                groupSequence.OnComplete(() =>
                {
                    var nextValue = value.Next(true);
                    var newBlock = _blockFactory.Create(nextValue, seed);
                    firstCell.Block = newBlock;
                    newBlock.transform.position = firstCell.Position;
                    newBlock.PlayScaleUpAnimation();
            
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
            
                _sequence.Append(groupSequence);
            }

            _sequence.OnComplete(() => IsAnimationActive = false);
            _sequence.Play();
        }

        public void Kill(bool complete)
        {
            _sequence.Kill(complete);
        }

    }
}